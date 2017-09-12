﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2017 Dapplo
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

namespace Dapplo.Owin
{
    /// <summary>
    /// An enum which assists in finding a location for the startup order of Owin modules
    /// </summary>
    public enum OwinModuleStartupOrders
    {
        /// <summary>
        /// Security settings
        /// </summary>
        Security = 0,
        /// <summary>
        /// Configuration
        /// </summary>
        Configuration = 1000,
        /// <summary>
        /// Services like SignalR
        /// </summary>
        Services = 10000,
        /// <summary>
        /// From here user specific modules are started
        /// </summary>
        User = 100000
    }
}