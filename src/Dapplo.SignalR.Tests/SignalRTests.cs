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

using System;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using Autofac;
using Dapplo.Addons.Bootstrapper;
using Dapplo.HttpExtensions;
using Dapplo.Log;
using Dapplo.Log.XUnit;
using Dapplo.Owin;
using Dapplo.SignalR.Tests.Configuration;
using Dapplo.SignalR.Tests.Hub;
using Dapplo.SignalR.Tests.Owin;
using Microsoft.AspNet.SignalR.Client;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Dapplo.SignalR.Tests
{
    public sealed class SignalRTests
    {
        private const string ApplicationName = "DapploSignalR";
        private static readonly LogSource Log = new LogSource();

        public SignalRTests(ITestOutputHelper testOutputHelper)
        {
            LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);

            // Disable cache, otherwise the server seems to respond even if it isn't running
            HttpExtensionsGlobals.HttpSettings.RequestCacheLevel = RequestCacheLevel.BypassCache;
        }

        [Fact]
        public async Task TestStartupAsync()
        {
            var applicationConfig = ApplicationConfigBuilder.
                Create()
                .WithApplicationName(ApplicationName)
                // Normally one would add Dapplo.Owin and Dapplo.SignalR dlls somewhere in a components or addons directory.
                // This would prevent to have a direct reference.
                .WithAssemblyPatterns("Dapplo*")
                .BuildApplicationConfig();

            using (var bootstrapper = new ApplicationBootstrapper(applicationConfig))
            {

                bootstrapper.Configure();

                // Startup the bootstrapper
                await bootstrapper.InitializeAsync();

                // Force mapping of IIniSubSection to IIniSection
                bootstrapper.Container.Resolve<IMyTestConfiguration>();

                var owinServer = bootstrapper.Container.Resolve<IOwinServer>();
                // Resetting the port to random
                owinServer.OwinConfiguration.AddListenerUri();

                // Startup the services
                await bootstrapper.StartupAsync();

                Assert.True(owinServer.IsListening, "Server not running!");

                var baseUri = owinServer.ListeningOn.FirstOrDefault();
                Assert.NotNull(baseUri);
                // Test request, we need to build the url
                var testUri = new Uri(baseUri).AppendSegments("Test");

                var result = await testUri.GetAsAsync<string>();
                Assert.Equal("Dapplo", result);

                var hubConnection = new HubConnection(baseUri, true);
                IHubProxy testHubProxy = hubConnection.CreateHubProxy("TestHub");
                await hubConnection.Start();

                // Test HubPipelineModules
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await testHubProxy.Invoke<string>("CreateException"));

                var hubPipelineTestModule = bootstrapper.Container.Resolve<HubPipelineTestModule>();
                Assert.NotNull(hubPipelineTestModule.LatestException);
                Assert.Equal(typeof(NotSupportedException),hubPipelineTestModule.LatestException.GetType());


                var signalrRresult = await testHubProxy.Invoke<string>("Hello", new TestType {Message = "World"});
                Assert.Equal("Hello World", signalrRresult);

                Log.Debug().WriteLine("Shutdown");
                await owinServer.ShutdownAsync();
                Assert.False(owinServer.IsListening, "Server still running!");

                Log.Debug().WriteLine("Test with a request that service is no longer available");
                await Assert.ThrowsAsync<TaskCanceledException>(async () =>
                {
                    // setup the request timeout to something small
                    var behavior = new HttpBehaviour {HttpSettings = {RequestTimeout = TimeSpan.FromMilliseconds(100)}};
                    behavior.MakeCurrent();
                    result = await testUri.GetAsAsync<string>();
                });

                Log.Debug().WriteLine("Starting again");
                await owinServer.StartupAsync();
                Assert.True(owinServer.IsListening, "Server not running!");

                await owinServer.ShutdownAsync();
                Assert.False(owinServer.IsListening, "Server still running!");
            }

        }
    }
}