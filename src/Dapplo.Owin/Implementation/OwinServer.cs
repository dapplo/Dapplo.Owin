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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using Dapplo.Addons;
using Dapplo.Addons.Services;
using Dapplo.Log;
using Dapplo.Owin.Configuration;
using Microsoft.Owin.Hosting;
using Owin;

#endregion

namespace Dapplo.Owin.Implementation
{
    /// <summary>
    ///     This class will can start an Owin server.
    ///  as a Startup-Action and will shut it down when the shutdown action is called.
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class OwinServer : ServiceNodeContainer<IOwinModule>, IOwinServer
    {
        private static readonly LogSource Log = new LogSource();
        private IDisposable _webApp;

        /// <summary>
        /// The Owin configuration
        /// </summary>
        public IOwinConfiguration OwinConfiguration { get; }

        /// <summary>
        /// Create an Owin Server
        /// </summary>
        /// <param name="owinConfiguration">IOwinConfiguration with the hostname and port to listen on, and some other parameters</param>
        /// <param name="owinModules">IEnumerable of Lazy IOwinModule and IOwinModuleMetadata</param>
        public OwinServer(
            IOwinConfiguration owinConfiguration,
            IEnumerable<Meta<IOwinModule, ServiceAttribute>> owinModules
            ) : base(owinModules)
        {
            OwinConfiguration = owinConfiguration;
        }

        /// <summary>
        ///     The server is listening on the following Uri
        /// </summary>
        public Uri ListeningOn { get; private set; }

        /// <summary>
        ///     Is the server running?
        /// </summary>
        public bool IsListening { get; private set; }

        /// <summary>
        ///     Stop the WebApp
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task ShutdownAsync(CancellationToken cancellationToken = default)
        {
            Log.Verbose().WriteLine("Stopping the Owin Server on {0}", ListeningOn);
            if (!ServiceNodes.Any())
            {
                Log.Info().WriteLine("No OwinModules to stop.");
                return;
            }

            await DeinitializeModules(ServiceNodes.Values.Where(node => !node.HasDependencies), cancellationToken).ConfigureAwait(false);
            IsListening = false;
            _webApp?.Dispose();
            _webApp = null;
        }

        /// <summary>
        ///     Start the WebApp
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task StartupAsync(CancellationToken cancellationToken = default)
        {
            if (!ServiceNodes.Any())
            {
                Log.Info().WriteLine("No OwinModules to start.");
                return;
            }

            // Logic to find the port to use, first take the one from the configuration
            int portToUse = OwinConfiguration.Port;
            // If there is no port given, find a free one
            if (OwinConfiguration.Port == 0)
            {
                portToUse = GetFreeListenerPort();
            }
            // build the uri to listen on
            ListeningOn = new Uri($"{OwinConfiguration.ListeningSchema}://{OwinConfiguration.Hostname}:{portToUse}");
            Log.Info().WriteLine("Starting WebApp on {0}", ListeningOn.AbsoluteUri);

            var rootModules = ServiceNodes.Values.Where(serviceNode => !serviceNode.HasPrerequisites).ToList();
            await InitializeModules(rootModules, cancellationToken).ConfigureAwait(false);

            _webApp = WebApp.Start(ListeningOn.AbsoluteUri, appBuilder =>
            {
                Log.Verbose().WriteLine("Starting WebApp.");
                ConfigureModules(appBuilder, rootModules);
            });
            IsListening = true;
        }

        /// <summary>
        /// Create a task for the InitializeAsync
        /// </summary>
        /// <param name="serviceNodes">IEnumerable with ServiceNode</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        private Task InitializeModules(IEnumerable<ServiceNode<IOwinModule>> serviceNodes, CancellationToken cancellationToken = default)
        {
            var tasks = new List<Task>();
            foreach (var serviceNode in serviceNodes)
            {
                Log.Debug().WriteLine("Initializing {0} ({1})", serviceNode.Details.Name, serviceNode.Service.GetType());

                var initializeTask = Task.Run(() => serviceNode.Service.InitializeAsync(this, cancellationToken), cancellationToken);

                if (serviceNode.Dependencies.Count > 0)
                {
                    // Recurse into InitializeModules
                    initializeTask = initializeTask.ContinueWith(task => InitializeModules(serviceNode.Dependencies, cancellationToken), cancellationToken).Unwrap();
                }
                if (!serviceNode.Details.SkipAwait)
                {
                    tasks.Add(initializeTask);
                }
            }

            return tasks.Count > 0 ? Task.WhenAll(tasks) : Task.CompletedTask;
        }

        /// <summary>
        /// Create a task for the InitializeAsync
        /// </summary>
        /// <param name="serviceNodes">IEnumerable with ServiceNode</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        private Task DeinitializeModules(IEnumerable<ServiceNode<IOwinModule>> serviceNodes, CancellationToken cancellationToken = default)
        {
            var tasks = new List<Task>();
            foreach (var serviceNode in serviceNodes)
            {
                Log.Debug().WriteLine("Deinitializing {0} ({1})", serviceNode.Details.Name, serviceNode.Service.GetType());

                var deinitializeTask = Task.Run(() => serviceNode.Service.DeinitializeAsync(this, cancellationToken), cancellationToken);

                if (serviceNode.Prerequisites.Count > 0)
                {
                    // Recurse into DeinitializeModules
                    deinitializeTask = deinitializeTask.ContinueWith(task => DeinitializeModules(serviceNode.Prerequisites, cancellationToken), cancellationToken).Unwrap();
                }

                if (!serviceNode.Details.SkipAwait)
                {
                    tasks.Add(deinitializeTask);
                }
            }

            return tasks.Count > 0 ? Task.WhenAll(tasks) : Task.CompletedTask;
        }

        /// <summary>
        /// Call Configure recursively
        /// </summary>
        /// <param name="appBuilder">IAppBuilder</param>
        /// <param name="serviceNodes">IEnumerable with ServiceNode</param>
        private void ConfigureModules(IAppBuilder appBuilder, IEnumerable<ServiceNode<IOwinModule>> serviceNodes)
        {
            foreach (var serviceNode in serviceNodes)
            {
                Log.Debug().WriteLine("Configuring {0} ({1})", serviceNode.Details.Name, serviceNode.Service.GetType());

                serviceNode.Service.Configure(this, appBuilder);
                ConfigureModules(appBuilder, serviceNode.Dependencies);
            }
        }

        /// <summary>
        ///     Returns an unused port, which savely can be used to listen to
        ///     A port of 0 in the list will have the following behaviour: https://msdn.microsoft.com/en-us/library/c6z86e63.aspx
        ///     If you do not care which local port is used, you can specify 0 for the port number. In this case, the service
        ///     provider will assign an available port number between 1024 and 5000.
        /// </summary>
        /// <param name="possiblePorts">An optional int array with ports, the routine will return the first free port.</param>
        /// <returns>A free port</returns>
        protected static int GetFreeListenerPort(int[] possiblePorts = null)
        {
            possiblePorts = possiblePorts ?? new[] {0};

            var resultingPort =  possiblePorts.Select(TryPort).FirstOrDefault(i => i > 0);
            if (resultingPort > 0)
            {
                return resultingPort;
            }
            var message = $"No free ports in the range {possiblePorts} found!";
            Log.Warn().WriteLine(message);
            throw new NotSupportedException(message);
        }

        /// <summary>
        /// Helper method to find an unused port
        /// </summary>
        /// <param name="portToCheck">0 for random, otherwise a specific</param>
        /// <returns>The actual port, or -1 of there isn't a free port</returns>
        private static int TryPort(int portToCheck)
        {
            var listener = new TcpListener(IPAddress.Loopback, portToCheck);
            try
            {
                listener.Start();
                // As the LocalEndpoint is of type EndPoint, this doesn't have the port, we need to cast it to IPEndPoint
                var port = ((IPEndPoint)listener.LocalEndpoint).Port;
                Log.Info().WriteLine("Found free listener port {0} for the WebApp.", port);
                return port;
            }
            catch
            {
                Log.Debug().WriteLine("Port {0} isn't free.", portToCheck);
            }
            finally
            {
                listener.Stop();
            }
            return -1;
        }
    }
}