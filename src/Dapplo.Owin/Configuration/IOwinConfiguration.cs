﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2018 Dapplo
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

#region using

using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using Dapplo.Ini;

#endregion

namespace Dapplo.Owin.Configuration
{
    /// <summary>
    ///     The Owin configuration container, this can be stored with Dapplo.Config
    /// </summary>
    public interface IOwinConfiguration : IIniSubSection
    {
        /// <summary>
        /// Schema for Owin to accept request on, e.g. http or https. Default is HTTP
        /// There is no support for https yet, so this probably doesn't work out of the box.
        /// </summary>
        [DefaultValue("http"), Description("Schema for Owin to accept request on, e.g. http (default) or https. For https, there is no help with certificates (yet?)."), DataMember(EmitDefaultValue = true)]
        string ListeningSchema { get; set; }

        /// <summary>
        /// Hostname for Owin to accept request on, default is localhost
        /// </summary>
        [DefaultValue("localhost"), Description("Host for Owin to accept request on."), DataMember(EmitDefaultValue = true)]
        string Hostname { get; set; }

        /// <summary>
        /// Port for owin to accept requests on, default is a random nummber greater than 10800
        /// </summary>
        [DefaultValue(0), Description("Port for Owin to accept request on, when this is 0 the first free port after 10800 is located."), DataMember(EmitDefaultValue = true)]
        int Port { get; set; }

        /// <summary>
        /// Specify if an error page should be shown
        /// </summary>
        [Description("Show an error page when something happens"), DefaultValue(false), DataMember(EmitDefaultValue = true)]
        bool UseErrorPage { get; set; }

        /// <summary>
        /// Enable Cross Origin calls
        /// </summary>
        [Description("Set this to true to allow cross origin calls, this is needed when a site is not served by the owin server, but uses this.")]
        [DefaultValue(true)]
        bool EnableCors { get; set; }

        /// <summary>
        /// Specify what AuthenticationScheme is used, default is none
        /// </summary>
        [Description("The Authentication scheme for Owin"), DefaultValue(AuthenticationSchemes.None), DataMember(EmitDefaultValue = true)]
        AuthenticationSchemes AuthenticationScheme { get; set; }
    }
}