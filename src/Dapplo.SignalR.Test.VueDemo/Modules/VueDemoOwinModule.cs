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
using System.Reflection;
using Dapplo.Addons;
using Dapplo.Owin;
using Dapplo.Owin.OwinModules;
using Dapplo.SignalR.Test.VueDemo.Utils;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Dapplo.SignalR.Test.VueDemo.Modules;

/// <summary>
///     Configure the VueDemo OWIN site
///     This will be automatically picked up by Dapplo.Owin
/// </summary>
[Service(nameof(VueDemoOwinModule), nameof(ConfigureOwinCors))]
public class VueDemoOwinModule : BaseOwinModule
{
    private readonly Func<Assembly, string, ExtendableEmbeddedResourceFileSystem> _fileSystemFactory;

    /// <summary>
    /// The constructor use for dependency injection
    /// </summary>
    /// <param name="fileSystemFactory">ExtendableEmbeddedResourceFileSystem</param>
    public VueDemoOwinModule(Func<Assembly, string, ExtendableEmbeddedResourceFileSystem> fileSystemFactory)
    {
        _fileSystemFactory = fileSystemFactory;
    }

    /// <summary>
    ///     Create a File-Server for the Vue-Demo files
    /// </summary>
    /// <param name="server">IOwinServer</param>
    /// <param name="appBuilder">IAppBuilder</param>
    public override void Configure(IOwinServer server, IAppBuilder appBuilder)
    {
        // Enable file server for the Call-ING files
        var vueDemoFileServerOption = new FileServerOptions
        {
            EnableDefaultFiles = false,
            RequestPath = new PathString("/vuedemo"),
            FileSystem = _fileSystemFactory(GetType().Assembly, $"{typeof(Startup).Namespace}.VueDemoSite"),
            EnableDirectoryBrowsing = false,
            DefaultFilesOptions =
            {
                DefaultFileNames =new[] {"index.html"}
            }
        };
        appBuilder.Use<CacheHeadersForStaticFilesOwinMiddleware>();
        appBuilder.UseFileServer(vueDemoFileServerOption);
        appBuilder.Use((owinContext, next) =>
        {
            return next().ContinueWith(x =>
            {
                var statusCode = owinContext.Response.StatusCode;
                if (statusCode < 300)
                {
                    return;
                }
                var request = owinContext.Request;
                var redirectUri = request.Uri.AbsoluteUri.Replace(request.Uri.PathAndQuery, $"{vueDemoFileServerOption.RequestPath}/error/{statusCode}.html");
                owinContext.Response.Redirect(redirectUri);
            });
        });
    }
}