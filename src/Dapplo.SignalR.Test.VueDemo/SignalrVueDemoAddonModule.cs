//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2019 Dapplo
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
using Dapplo.Config.Ini;
using Dapplo.Owin;
using Dapplo.Owin.Configuration;
using Dapplo.SignalR.Configuration;
using Dapplo.SignalR.Test.VueDemo.Configuration;
using Dapplo.SignalR.Test.VueDemo.Configuration.Impl;
using Dapplo.SignalR.Test.VueDemo.Hubs;
using Dapplo.SignalR.Test.VueDemo.Model;
using Dapplo.SignalR.Test.VueDemo.Model.Impl;
using Dapplo.SignalR.Test.VueDemo.Modules;
using Dapplo.SignalR.Test.VueDemo.Ui;
using Dapplo.SignalR.Test.VueDemo.Utils;

namespace Dapplo.SignalR.Test.VueDemo
{
    /// <summary>
    /// Configure Autofac
    /// </summary>
    public class SignalrVueDemoAddonModule : AddonModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Create a default configuration, if none exists
            builder.Register(context => IniFileConfigBuilder.Create().BuildIniFileConfig())
                .IfNotRegistered(typeof(IniFileConfig))
                .As<IniFileConfig>()
                .SingleInstance();

            builder.RegisterType<IniFileContainer>()
                .AsSelf()
                .SingleInstance();

            builder
                .RegisterType<MainWindow>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<ConfigService>()
                .As<IService>()
                .SingleInstance();
            builder
                .RegisterType<MyVueModel>()
                .As<IMyVueModel>()
                .SingleInstance();

            builder
                .RegisterType<OwinService>()
                .As<IService>()
                .SingleInstance();

            builder
                .RegisterType<VueDemoOwinModule>()
                .As<IOwinModule>()
                .SingleInstance();

            builder
                .RegisterType<WebserverConfiguration>()
                .As<IWebserverConfiguration>()
                .As<IOwinConfiguration>()
                .As<ISignalRConfiguration>()
                .As<IIniSection>()
                .SingleInstance();

            builder
                .RegisterType<ExtendableEmbeddedResourceFileSystem>()
                .AsSelf();

            builder
                .RegisterType<EmbeddedResourceFileInfo>()
                .AsSelf();

            builder
                .RegisterType<VueHub>()
                .AsSelf()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
