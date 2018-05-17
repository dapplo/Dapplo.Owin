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

using Autofac;
using Dapplo.Owin.Implementation;
using Dapplo.Owin.OwinModules;

namespace Dapplo.Owin
{
    /// <summary>
    /// Adds an IOwinModule to configure Owin in Autofac
    /// </summary>
    public class OwinAutofacModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<OwinServer>()
                .As<IOwinServer>()
                .SingleInstance();
            builder
                .RegisterType<ConfigureOwinAuthentication>()
                .As<IOwinModule>()
                .SingleInstance();
            builder
                .RegisterType<ConfigureOwinAutofac>()
                .As<IOwinModule>()
                .SingleInstance();
            builder
                .RegisterType<ConfigureOwinCors>()
                .As<IOwinModule>()
                .SingleInstance();
            builder
                .RegisterType<ConfigureOwinErrorPage>()
                .As<IOwinModule>()
                .SingleInstance();
        }
    }
}
