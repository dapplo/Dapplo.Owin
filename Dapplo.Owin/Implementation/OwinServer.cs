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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Microsoft.Owin.Hosting;
using System.Net.Sockets;
using Dapplo.LogFacade;
using System.Net;

namespace Dapplo.Owin.Implementation
{
	/// <summary>
	/// This class will start an OwinService as a Startup-Action and will shut it down when the shutdown action is called.
	/// </summary>
	[StartupAction, ShutdownAction]
	public class OwinServer : IOwinServer
	{
		private static readonly LogSource Log = new LogSource();
		private IDisposable _webApp;

		[Import]
		private IServiceLocator ServiceLocator
		{
			get;
			set;
		}

		[Import]
		private IOwinConfiguration OwinConfiguration
		{
			get;
			set;
		}

		[ImportMany]
		private IEnumerable<Lazy<IOwinConfigure, IOwinConfigureMetadata>> OwinStartups
		{
			get;
			// ReSharper disable once UnusedAutoPropertyAccessor.Local
			set;
		}

		/// <summary>
		/// The server is listening on the following Uri
		/// </summary>
		public Uri ListeningOn
		{
			get;
			private set;
		}

		/// <summary>
		/// Is the server running?
		/// </summary>
		public bool IsListening
		{
			get;
			private set;
		}

		/// <summary>
		/// Stop the WebApp
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>Task</returns>
		public async Task ShutdownAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			Log.Verbose().WriteLine("Stopping the Owin Server on {0}", ListeningOn);
			await Task.Run(() => StopWebApp(), cancellationToken);
		}

		/// <summary>
		/// Start the WebApp
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>Task</returns>
		public async Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ServiceLocator.Export<IOwinServer>(this);
			await Task.Run(() => StartWebApp(), cancellationToken);
		}

		/// <summary>
		/// The actual starting of the WebApp is done here
		/// </summary>
		private void StartWebApp()
		{
			var orderedOwinStartups = from export in OwinStartups orderby export.Metadata.StartupOrder ascending select export;
			if (orderedOwinStartups.Count() == 0)
			{
				Log.Info().WriteLine("No OwinStartups to start.");
				return;
			}

			// If there is no port given, find a free one and store this in the configuration
			if (OwinConfiguration.Port == 0)
			{
				OwinConfiguration.Port = GetFreeListenerPort(new[] { 0 });
			}
			ListeningOn = new Uri($"http://{OwinConfiguration.Hostname}:{OwinConfiguration.Port}");
			Log.Info().WriteLine("Starting WebApp on {0}", ListeningOn.AbsoluteUri);

			_webApp = WebApp.Start(ListeningOn.AbsoluteUri, appBuilder =>
			{
				foreach (var owinStartup in orderedOwinStartups)
				{
					owinStartup.Value.ConfigureOwin(this, appBuilder);
				}
			});
			IsListening = true;
		}

		private void StopWebApp()
		{
			IsListening = false;
			_webApp?.Dispose();
			_webApp = null;
		}

		/// <summary>
		/// Returns an unused port, which savely can be used to listen to
		/// A port of 0 in the list will have the following behaviour: https://msdn.microsoft.com/en-us/library/c6z86e63.aspx
		/// If you do not care which local port is used, you can specify 0 for the port number. In this case, the service provider will assign an available port number between 1024 and 5000.
		/// </summary>
		/// <param name="possiblePorts">An int array with ports, the routine will return the first free port.</param>
		/// <returns>A free port</returns>
		private static int GetFreeListenerPort(int[] possiblePorts)
		{
			possiblePorts = possiblePorts ?? new int[] { 0 };

			foreach (var portToCheck in possiblePorts)
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
					continue;
				}
				finally
				{
					listener.Stop();
				}
			}
			var message = $"No free ports in the range {possiblePorts} found!";
			Log.Warn().WriteLine(message);
			throw new ApplicationException(message);
		}
	}
}
