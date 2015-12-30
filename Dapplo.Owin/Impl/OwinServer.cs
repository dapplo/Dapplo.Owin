/*
 * dapplo - building blocks for desktop applications
 * Copyright (C) Dapplo 2015-2016
 * 
 * For more information see: http://dapplo.net/
 * dapplo repositories are hosted on GitHub: https://github.com/dapplo
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Microsoft.Owin.Hosting;

namespace Dapplo.Owin.Impl
{
	/// <summary>
	/// This class will start an OwinService as a Startup-Action and will shut it down when the shutdown action is called.
	/// </summary>
	[StartupAction, ShutdownAction]
	public class OwinServer : IStartupAction, IShutdownAction
	{
		private IDisposable _webApp;

		[Import]
		private IOwinConfiguration OwinConfiguration
		{
			get;
			set;
		}

		[ImportMany]
		private IEnumerable<Lazy<IOwinStartup, IOwinStartupMetadata>> OwinStartups
		{
			get;
			// ReSharper disable once UnusedAutoPropertyAccessor.Local
			set;
		}

		public Task ShutdownAsync(CancellationToken token = default(CancellationToken))
		{
			StopWebApp();
			return Task.FromResult(true);
		}

		public Task StartAsync(CancellationToken token = default(CancellationToken))
		{
			StartWebApp();
            return Task.FromResult(true);
		}


		private void StartWebApp()
		{
			_webApp = WebApp.Start($"http://{OwinConfiguration.Hostname}:{OwinConfiguration.Port}", appBuilder =>
			{
				var orderedOwinStartups = from export in OwinStartups orderby export.Metadata.StartupOrder ascending select export;
				foreach (var owinStartup in orderedOwinStartups)
				{
					owinStartup.Value.Configuration(appBuilder);
				}
			});
		}

		private void StopWebApp()
		{
			_webApp?.Dispose();
			_webApp = null;
		}
	}
}
