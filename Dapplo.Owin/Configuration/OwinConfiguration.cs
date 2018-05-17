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

using System.Net;

#endregion

namespace Dapplo.Owin.Configuration
{
    /// <summary>
    ///     A basic implementation of the IOwinConfiguration, not needed when using Dapplo
    /// </summary>
    public class OwinConfiguration : IOwinConfiguration
    {
        /// <inheritdoc />
        public string ListeningSchema { get; set; } = "http";

        /// <inheritdoc />
        public string Hostname { get; set; } = "localhost";

        /// <inheritdoc />
        public int Port { get; set; }

        /// <inheritdoc />
        public bool UseErrorPage { get; set; } = true;

        /// <inheritdoc />
        public bool EnableCors { get; set; } = true;

        /// <inheritdoc />
        public AuthenticationSchemes AuthenticationScheme { get; set; } = AuthenticationSchemes.None;
    }
}