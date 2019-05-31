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

using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;

namespace Dapplo.Owin.Configuration
{
    /// <summary>
    ///     The Owin configuration container, this can be stored with Dapplo.Config
    /// </summary>
    public interface IOwinConfiguration
    {
        /// <summary>
        /// Passed to the startoptions of the Owin webapp as urls to listen on
        /// </summary>
        [Description("Urls for the Owin server to listen on.")]
        IList<string> ListeningUrls { get; set; }

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