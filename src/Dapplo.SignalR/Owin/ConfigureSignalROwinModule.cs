#region Dapplo License

//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2018 Dapplo
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

using System.Collections.Generic;
using Autofac;
using Autofac.Integration.SignalR;
using Dapplo.Addons;
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
    [Service(nameof(ConfigureSignalROwinModule))]
    public class ConfigureSignalROwinModule : BaseOwinModule
    {
        private static readonly LogSource Log = new LogSource();
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IEnumerable<IHubPipelineModule> _hubPipelineModules;
        private readonly IHubActivator _hubActivator;

        /// <summary>
        /// The configuration for SignalR
        /// </summary>
        protected ISignalRConfiguration SignalRConfiguration { get; }

        /// <inheritdoc />
        public ConfigureSignalROwinModule(
            ILifetimeScope lifetimeScope,
            ISignalRConfiguration signalRConfiguration,
            IEnumerable<IHubPipelineModule> hubPipelineModules,
            IHubActivator hubActivator = null)
        {
            _lifetimeScope = lifetimeScope;
            _hubPipelineModules = hubPipelineModules;
            _hubActivator = hubActivator;
            SignalRConfiguration = signalRConfiguration;
        }

        /// <summary>
        ///     Configure Owin for SignalR
        /// </summary>
        /// <param name="server"></param>
        /// <param name="appBuilder"></param>
        public override void Configure(IOwinServer server, IAppBuilder appBuilder)
        {
            Log.Verbose().WriteLine("Activating SignalR, EnableJavaEnableJavaScriptProxies={0}, EnableDetailedErrors={1}, UseDummyPerformanceCounter={2}, UseErrorLogger={3}", SignalRConfiguration.EnableJavaEnableJavaScriptProxies, SignalRConfiguration.EnableDetailedErrors, SignalRConfiguration.UseDummyPerformanceCounter, SignalRConfiguration.EnableExceptionLogger);

            // Needed to make sure we can start & stop it multiple times
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(_lifetimeScope);

            if (_hubActivator != null)
            {
                Log.Verbose().WriteLine("Overriding the DefaultHubActivator");
                // Register our own IHubActivator, so we can use dependency injection
                GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => _hubActivator);
            }

            // Register HubPipelineExceptionLoggerModule if error logger is enabled
            if (SignalRConfiguration.EnableExceptionLogger)
            {
                Log.Info().WriteLine("Added PipelineModule HubPipelineExceptionLoggerModule.");
                GlobalHost.HubPipeline.AddModule(new HubPipelineExceptionLoggerModule());
            }

            // Register pipeline modules
            foreach (var hubPipelineModule in _hubPipelineModules)
            {
                Log.Verbose().WriteLine("Adding PipelineModule {0}.", hubPipelineModule.GetType());
                GlobalHost.HubPipeline.AddModule(hubPipelineModule);
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