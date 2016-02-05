/*
	Dapplo - building blocks for desktop applications
	Copyright (C) 2015-2016 Dapplo

	For more information see: http://dapplo.net/
	Dapplo repositories are hosted on GitHub: https://github.com/dapplo

	This file is part of Dapplo.Owin.

	Dapplo.Owin is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Dapplo.Owin is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Dapplo.Owin. If not, see <http://www.gnu.org/licenses/>.
 */

using Dapplo.Addons.Implementation;
using Dapplo.Config.Ini;
using Dapplo.HttpExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
namespace Dapplo.Owin.Tests
{
	[TestClass]
	public class OwinTests
	{
		private const string ApplicationName = "DapploOwin";

		[TestMethod]
		public async Task TestStartup()
		{
			using (var bootstrapper = new ApplicationBootstrapper(ApplicationName))
			{
				var iniConfig = new IniConfig(ApplicationName, "test");
				var owinConfig = await iniConfig.RegisterAndGetAsync<IOwinConfiguration>();

				bootstrapper.Add(typeof(OwinStartupTest));

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
				Assert.AreEqual("Dapplo", result);
			}
		}
	}
}
