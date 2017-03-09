using System;
using System.ComponentModel.Composition;
using Dapplo.Addons;
using Microsoft.AspNet.SignalR.Hubs;

namespace Dapplo.SignalR
{
	/// <summary>
	/// This IHubActivator implementation uses the Dapplo.Addons to enable dependency injection
	/// </summary>
	[Export(typeof(IHubActivator))]
	public class HubActivator : IHubActivator
	{
		[Import]
		private IServiceLocator ServiceLocator { get; set; }

		[Import]
		private IServiceExporter ServiceExporter { get; set; }

		/// <summary>
		/// Lookup or create a IHub and inject it.
		/// </summary>
		/// <param name="descriptor">HubDescriptor</param>
		/// <returns>IHub</returns>
		public IHub Create(HubDescriptor descriptor)
		{
			// Have the base implementation create the hub
			var hub = ServiceLocator.GetExport(descriptor.HubType) as IHub;
			if (hub != null)
			{
				return hub;
			}
			hub = Activator.CreateInstance(descriptor.HubType) as IHub;
			if (hub == null)
			{
				return null;
			}
			// Use the IServiceLocator to inject dependencies
			ServiceLocator.FillImports(hub);
			// Use the IServiceExporter to export the hub
			ServiceExporter.Export(descriptor.HubType, hub);
			return hub;
		}
	}
}
