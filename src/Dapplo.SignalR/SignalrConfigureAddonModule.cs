#region Dapplo License

//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2019 Dapplo
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

using Autofac;
using Dapplo.Addons;
using Dapplo.Owin;
using Dapplo.SignalR.Configuration;
using Dapplo.SignalR.Owin;
using Dapplo.SignalR.Utils;
using Microsoft.AspNet.SignalR.Hubs;

namespace Dapplo.SignalR
{
    /// <summary>
    /// Adds an IOwinModule to configure SignalR to Autofac
    /// </summary>
    public class SignalrConfigureAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<SignalRConfigurationImpl>()
                .IfNotRegistered(typeof(ISignalRConfiguration))
                .As<ISignalRConfiguration>();
            builder
                .RegisterType<ConfigureSignalROwinModule>()
                .As<IOwinModule>()
                .SingleInstance();
            builder
                .RegisterType<HubActivator>()
                .As<IHubActivator>()
                .SingleInstance();
        }
    }
}
