//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2018 Dapplo
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

using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Dapplo.Addons.Bootstrapper;
using Dapplo.HttpExtensions;
using Dapplo.Log;
using Xunit;
using Xunit.Abstractions;
using Dapplo.Log.XUnit;
using Dapplo.Owin.Tests.Configuration;

#endregion

namespace Dapplo.Owin.Tests
{
    /// <summary>
    /// Test case for OWIN with Autofac
    /// </summary>
    public sealed class OwinAutofacTests
    {
        private const string ApplicationName = "DapploOwin";

        public OwinAutofacTests(ITestOutputHelper testOutputHelper)
        {
            LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);
            // Disable cache, otherwise the server seems to respond even if it isn't running
            HttpExtensionsGlobals.HttpSettings.RequestCacheLevel = RequestCacheLevel.BypassCache;
        }

        [Fact]
        public async Task TestStartupShutdownAsync()
        {
            var applicationConfig = ApplicationConfigBuilder
                .Create()
                .WithApplicationName(ApplicationName)
                // Normally one would add Dapplo.Owin and Dapplo.SignalR dlls somewhere in a components or addons directory.
                // This would prevent to have a direct reference.
                .WithAssemblyPatterns("Dapplo*")
                .BuildApplicationConfig();

            using (var bootstrapper = new ApplicationBootstrapper(applicationConfig))
            {
                bootstrapper.Configure();

                await bootstrapper.InitializeAsync();

                // Force mapping of IIniSubSection to IIniSection
                bootstrapper.Container.Resolve<IMyTestConfiguration>();

                await bootstrapper.StartupAsync();

                var owinServer = bootstrapper.Container.Resolve<IOwinServer>();
                Assert.True(owinServer.IsListening, "Server not running!");

                // Resetting the port to random
                owinServer.OwinConfiguration.Port = 0;

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

                await owinServer.StartupAsync();
                Assert.True(owinServer.IsListening, "Server not running!");

                await owinServer.ShutdownAsync();
                Assert.False(owinServer.IsListening, "Server still running!");
            }

        }
    }
}