//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2017 Dapplo
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

using System;
using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using Dapplo.Addons.Bootstrapper;
using Dapplo.Addons.Bootstrapper.ExportProviders;
using Dapplo.Ini;
using Dapplo.HttpExtensions;
using Dapplo.Log;
using Xunit;
using Xunit.Abstractions;
using Dapplo.Log.XUnit;
using Dapplo.Owin.Tests.Configuration;
using Dapplo.Owin.Tests.Modules;
using Dapplo.Owin.Tests.Owin;
using Nito.AsyncEx;

#endregion

namespace Dapplo.Owin.Tests
{
	public class OwinTests
	{
		private const string ApplicationName = "DapploOwin";

		private static readonly AsyncLazy<ApplicationBootstrapper> Bootstrapper = new AsyncLazy<ApplicationBootstrapper>(async () =>
		{
			var bootstrapper = new ApplicationBootstrapper(ApplicationName);

			bootstrapper.Add(typeof(TestMiddlewareOwinModule));

			// Make sure IniConfig can resolve and find IMyTestConfiguration
			var iniConfig = new IniConfig(ApplicationName, ApplicationName);
			// TODO: Find a solution where register is not needed?
			await iniConfig.RegisterAndGetAsync<IMyTestConfiguration>();
			var exportProvider = new ServiceProviderExportProvider(iniConfig, bootstrapper);
			bootstrapper.ExportProviders.Add(exportProvider);

			// Normally one would add Dapplo.Owin and Dapplo.SignalR dlls, without having a direct reference:
			// e.g.: bootstrapper.AddScanDirectory(@"..\..\..\Dapplo.Owin\bin\Debug");
			// e.g.: bootstrapper.AddScanDirectory(@"..\..\..\Dapplo.SignalR\bin\Debug");
			bootstrapper.FindAndLoadAssemblies("Dapplo*");
			// Start the composition
			await bootstrapper.RunAsync();
			return bootstrapper;
		});


		public OwinTests(ITestOutputHelper testOutputHelper)
		{
			LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);
			// Disable cache, otherwise the server seems to respond even if it isn't running
			HttpExtensionsGlobals.HttpSettings.RequestCacheLevel = RequestCacheLevel.BypassCache;
		}

		public void Dispose()
		{
			Bootstrapper.GetAwaiter().GetResult().Dispose();
		}

		[Fact]
		public async Task TestStartupShutdownAsync()
		{
			var bootstrapper = await Bootstrapper;

			var owinServer = bootstrapper.GetExport<IOwinServer>().Value;
			Assert.True(owinServer.IsListening, "Server not running!");
			// Test request, we need to build the url
			var testUri = owinServer.ListeningOn.AppendSegments("Test");

			var result = await testUri.GetAsAsync<string>();
			Assert.Equal("Dapplo", result);

			await owinServer.ShutdownAsync();
			Assert.False(owinServer.IsListening, "Server still running!");

			await Assert.ThrowsAsync<HttpRequestException>(async () =>
			{
				result = await testUri.GetAsAsync<string>();
			});

			await owinServer.StartAsync();
			Assert.True(owinServer.IsListening, "Server not running!");

			await owinServer.ShutdownAsync();
			Assert.False(owinServer.IsListening, "Server still running!");
		}
	}
}