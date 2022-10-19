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

using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Dapplo.Config.Ini;
using Dapplo.Owin.Configuration;
using Dapplo.SignalR.Configuration;

namespace Dapplo.SignalR.Test.VueDemo.Configuration;

/// <summary>
/// The configuration for the web-server (owin)
/// </summary>
[IniSection("Webserver")]
[Description("The configuration for the web-server (owin)")]
public interface IWebserverConfiguration : IIniSection, IOwinConfiguration, ISignalRConfiguration
{
    /// <summary>
    /// Force the listening uri
    /// </summary>
    [DefaultValue("http://localhost:8380")]
    [Description("Urls for the Owin server to listen on.")]
    new IList<string> ListeningUrls { get; set; }

    /// <summary>
    /// Force AuthenticationScheme to have Negotiate as default
    /// </summary>
    [Description("The Authentication scheme for Owin")]
    [DefaultValue(AuthenticationSchemes.Negotiate)]
    [IniPropertyBehavior(Read = false, Write = false)]
    new AuthenticationSchemes AuthenticationScheme { get; set; }
}