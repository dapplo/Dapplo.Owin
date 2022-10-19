//  Dapplo - building blocks for desktop applications
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

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Dapplo.Addons.Bootstrapper;
using Dapplo.Log;
using Dapplo.Log.Loggers;
using Dapplo.SignalR.Test.VueDemo.Ui;

namespace Dapplo.SignalR.Test.VueDemo;

public class Startup
{
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Debug);
#endif
        var applicationConfig = ApplicationConfigBuilder.
            Create()
            .WithApplicationName("VueDemo")
            .WithoutCopyOfEmbeddedAssemblies()
            .WithAssemblyPatterns("Dapplo.*")
            .BuildApplicationConfig();
        using (var bootstrapper = new ApplicationBootstrapper(applicationConfig))
        {
            bootstrapper.Configure();

            bootstrapper.StartupAsync().Wait();

            Task.Delay(1000).Wait();
            Process.Start("http://localhost:8380/vuedemo/index.html");

            var application = new Application();
            var mainWindow = bootstrapper.Container.Resolve<MainWindow>();
            application.Run(mainWindow);
            bootstrapper.ShutdownAsync().Wait();
        }
    }
}