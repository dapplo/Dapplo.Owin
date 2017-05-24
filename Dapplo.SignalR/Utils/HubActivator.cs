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

using System;
using System.ComponentModel.Composition;
using Dapplo.Addons;
using Microsoft.AspNet.SignalR.Hubs;

#endregion

namespace Dapplo.SignalR.Utils
{
    /// <summary>
    ///     This IHubActivator implementation uses the Dapplo.Addons to enable dependency injection
    /// </summary>
    [Export(typeof(IHubActivator))]
    public class HubActivator : IHubActivator
    {
        [Import]
        private IMefServiceLocator MefServiceLocator { get; set; }

        [Import]
        private IDependencyProvider DependencyProvider { get; set; }

        [Import]
        private IServiceExporter ServiceExporter { get; set; }

        /// <summary>
        ///     Lookup or create a IHub and inject it.
        /// </summary>
        /// <param name="descriptor">HubDescriptor</param>
        /// <returns>IHub</returns>
        public IHub Create(HubDescriptor descriptor)
        {
            // Have the base implementation create the hub
            var hub = MefServiceLocator.GetExport(descriptor.HubType) as IHub;
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
            DependencyProvider.ProvideDependencies(hub);
            // Use the IServiceExporter to export the hub
            ServiceExporter.Export(descriptor.HubType, hub);
            return hub;
        }
    }
}