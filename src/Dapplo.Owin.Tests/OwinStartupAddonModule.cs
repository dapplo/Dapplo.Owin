﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2022 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Owin
// 
//  Dapplo.Owin is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Owin is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Owin. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using Autofac;
using Dapplo.Addons;
using Dapplo.Config;
using Dapplo.Owin.Tests.Configuration;
using Dapplo.Owin.Tests.Owin;

namespace Dapplo.Owin.Tests;

/// <summary>
/// Configure Autofac with OWIN for the test
/// </summary>
public class OwinStartupAddonModule : AddonModule
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register(c => DictionaryConfiguration<IMyTestConfiguration>.Create())
            .IfNotRegistered(typeof(IMyTestConfiguration))
            .As<IMyTestConfiguration>()
            .SingleInstance();

        builder
            .RegisterType<OwinService>()
            .As<IService>()
            .SingleInstance();

        builder
            .RegisterType<TestMiddlewareOwinModule>()
            .As<IOwinModule>()
            .SingleInstance();
    }
}