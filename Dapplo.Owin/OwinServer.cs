using Dapplo.Addons;
using Microsoft.Owin.Hosting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace Dapplo.Owin
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
			_webApp = WebApp.Start($"http://{OwinConfiguration.Host}:{OwinConfiguration.Port}", (appBuilder) =>
			{
				var orderedOwinStartups = from export in OwinStartups orderby export.Metadata.StartupOrder ascending select export;
				foreach (var owinStartup in orderedOwinStartups)
				{
					try
					{
						owinStartup.Value.Configuration(appBuilder);
					}
					catch
					{
						// Nothing
					}
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
