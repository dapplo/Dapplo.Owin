﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Owin
// 
//  Dapplo.Owin is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Owin is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Owin. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System.Threading.Tasks;
using Dapplo.Addons.Bootstrapper;
using Dapplo.Config.Ini;
using Dapplo.HttpExtensions;
using Dapplo.LogFacade;
using Xunit;
using Xunit.Abstractions;
using Dapplo.Log.XUnit;

#endregion

namespace Dapplo.Owin.Tests
{
	public class OwinTests
	{
		private const string ApplicationName = "DapploOwin";

		public OwinTests(ITestOutputHelper testOutputHelper)
		{
			XUnitLogger.RegisterLogger(testOutputHelper, LogLevels.Verbose);
		}

		[Fact]
		public async Task TestStartupAsync()
		{
			using (var bootstrapper = new ApplicationBootstrapper(ApplicationName))
			{
				var iniConfig = new IniConfig(ApplicationName, "test");
				await iniConfig.RegisterAndGetAsync<IOwinConfiguration>();

				bootstrapper.Add(typeof (OwinConfigurationTest));

				// Add owin server project, without having a direct reference.
#if DEBUG
				bootstrapper.Add(@"..\..\..\Dapplo.Owin\bin\Debug", "Dapplo.*.dll");
#else
				bootstrapper.Add(@"..\..\..\Dapplo.Owin\bin\Release", "Dapplo.*.dll");
#endif
				// Start the composition
				await bootstrapper.RunAsync();

				var owinServer = bootstrapper.GetExport<IOwinServer>().Value;
				// Test request, we need to build the url
				var testUri = owinServer.ListeningOn.AppendSegments("Test");
				var result = await testUri.GetAsAsync<string>();
				Assert.Equal("Dapplo", result);
			}
		}
	}
}