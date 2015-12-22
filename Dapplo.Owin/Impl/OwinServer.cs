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
			_webApp = WebApp.Start($"http://{OwinConfiguration.Host}:{OwinConfiguration.Port}", appBuilder =>
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
