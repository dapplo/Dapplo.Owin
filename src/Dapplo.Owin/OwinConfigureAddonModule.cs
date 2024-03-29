﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2022 Dapplo
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

using Autofac;
using Dapplo.Addons;
using Dapplo.Config;
using Dapplo.Owin.Configuration;
using Dapplo.Owin.Implementation;
using Dapplo.Owin.OwinModules;

namespace Dapplo.Owin;

/// <summary>
/// Adds an IOwinModule to configure Owin in Autofac
/// </summary>
public class OwinConfigureAddonModule : AddonModule
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register(_ => DictionaryConfiguration<IOwinConfiguration>.Create())
            .IfNotRegistered(typeof(IOwinConfiguration))
            .As<IOwinConfiguration>();
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