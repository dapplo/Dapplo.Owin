#region Dapplo License

//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2017 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.SignalR
// 
//  Dapplo.SignalR is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.SignalR is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.SignalR. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System.ComponentModel.Composition;
using Dapplo.Log;
using Dapplo.Owin;
using Dapplo.SignalR.Configuration;
using Dapplo.SignalR.Utils;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using Owin;

#endregion

namespace Dapplo.SignalR.Owin
{
    /// <summary>
    ///     SignalR generic OWIN configuration
    /// </summary>
    [OwinModule(StartupOrder = (int)OwinModuleStartupOrders.Services)]
    public class ConfigureSignalROwinModule : BaseOwinModule
    {
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        /// The configuration for SignalR
        /// </summary>
        [Import]
        protected ISignalRConfiguration SignalRConfiguration { get; set; }

        /// <summary>
        /// The IHubActivator which overrides the DefaultHubActivator
        /// </summary>
        [Import]
        protected IHubActivator HubActivator { get; set; }

        /// <summary>
        ///     Configure Owin for SignalR
        /// </summary>
        /// <param name="server"></param>
        /// <param name="appBuilder"></param>
        public override void Configure(IOwinServer server, IAppBuilder appBuilder)
        {
            Log.Verbose().WriteLine("Activating SignalR, EnableJavaEnableJavaScriptProxies={0}, EnableDetailedErrors={1}, UseDummyPerformanceCounter={2}, UseErrorLogger={3}", SignalRConfiguration.EnableJavaEnableJavaScriptProxies, SignalRConfiguration.EnableDetailedErrors, SignalRConfiguration.UseDummyPerformanceCounter, SignalRConfiguration.UseErrorLogger);

            // Needed to make sure we can start & stop it multiple times
            GlobalHost.DependencyResolver = new DefaultDependencyResolver();

            // Activate error logger if this is enabled
            if (SignalRConfiguration.UseErrorLogger)
            {
                GlobalHost.HubPipeline.AddModule(new ExceptionLoggerHubPipelineModule());
            }

            if (HubActivator != null)
            {
                Log.Verbose().WriteLine("Overriding the DefaultHubActivator");
                // Register our own IHubActivator, so we can use dependency injection
                GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => HubActivator);
            }

            // Register a dummy IPerformanceCounterManager as a workaround
            if (SignalRConfiguration.UseDummyPerformanceCounter)
            {
                GlobalHost.DependencyResolver.Register(typeof(IPerformanceCounterManager), () => new DummyPerformanceCounterManager());
            }

            // Register the SignalRContractResolver, which solves camelCase issues
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SignalRContractResolver()
            };
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            // Add SignalR
            var hubConfiguration = new HubConfiguration
            {
                EnableJavaScriptProxies = SignalRConfiguration.EnableJavaEnableJavaScriptProxies,
                EnableDetailedErrors = SignalRConfiguration.EnableDetailedErrors,
                Resolver = GlobalHost.DependencyResolver
            };
            appBuilder.Map("/signalr", map =>
            {
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}