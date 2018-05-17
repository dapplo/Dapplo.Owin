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

using System;
using Autofac;
using Dapplo.Log;
using Microsoft.AspNet.SignalR.Hubs;

#endregion

namespace Dapplo.SignalR.Utils
{
    /// <summary>
    ///     This IHubActivator implementation uses the Dapplo.Addons to enable dependency injection
    /// </summary>
    public class HubActivator : IHubActivator
    {
        private static readonly LogSource Log = new LogSource();
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Default constructor for the HubActivator
        /// </summary>
        /// <param name="lifetimeScope">ILifetimeScope from Autofac</param>
        public HubActivator(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        ///     Lookup or create a IHub and inject it.
        /// </summary>
        /// <param name="descriptor">HubDescriptor</param>
        /// <returns>IHub</returns>
        public IHub Create(HubDescriptor descriptor)
        {
            // Use the container to locate the hub
            if (_lifetimeScope.TryResolve(descriptor.HubType, out var hubService) && hubService is IHub hub)
            {
                Log.Verbose().WriteLine("Hub {0} was resolved.", descriptor.HubType);
                return hub;
            }
            Log.Warn().WriteLine("Type {0} was not exported, will be instanciated via the type Activator.", descriptor.HubType);
            hub = Activator.CreateInstance(descriptor.HubType) as IHub;
            if (hub == null)
            {
                Log.Warn().WriteLine("Type {0} could not be instanciated.", descriptor.HubType);
                return null;
            }

            // Use the container to inject dependencies
            _lifetimeScope.InjectProperties(hub);
            return hub;
        }
    }
}