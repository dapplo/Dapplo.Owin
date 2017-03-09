using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapplo.Addons;
using Dapplo.Log;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using Dapplo.Owin;

namespace Dapplo.SignalR
{
	/// <summary>
	///     SignalR generic OWIN configuration
	/// </summary>
	[OwinModule]
	public class SignalRStartup : SimpleOwinModule
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		private ISignalRConfiguration SignalRConfiguration { get; set; }

		[Import]
		private IServiceLocator ServiceLocator { get; set; }

		[Import]
		private IHubActivator HubActivator { get; set; }

		/// <summary>
		///     Configure OWIN with:
		///     * IntegratedWindowsAuthentication
		///     * SignalR
		///     * CORS
		/// </summary>
		/// <param name="server"></param>
		/// <param name="appBuilder"></param>
		public override void Configure(IOwinServer server, IAppBuilder appBuilder)
		{
			// Register the SignalRContractResolver, which solves camelCase issues
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new SignalRContractResolver()
			};
			var serializer = JsonSerializer.Create(settings);
			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

			// Add Signal R and enable detailed errors
			appBuilder.MapSignalR(new HubConfiguration
			{
				EnableJavaScriptProxies = SignalRConfiguration.EnableJavaEnableJavaScriptProxies,
				EnableDetailedErrors = SignalRConfiguration.EnableDetailedErrors
			});

			// Register our own IHubActivator, so we can use dependency injection
			GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => HubActivator);
		}
	}
}
