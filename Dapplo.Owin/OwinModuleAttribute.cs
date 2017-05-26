//  Dapplo - building blocks for desktop applications
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

#region using

using System;
using System.ComponentModel.Composition;
using Dapplo.Addons;

#endregion

namespace Dapplo.Owin
{
    /// <summary>
    ///     Set this attribute on a class that implements Dapplo.Owin.IOwinConfigure have it called when Owin is started and
    ///     needs to be configured.
    ///     This attribute will make sure it's available via MEF (dependency injection)
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public class OwinModuleAttribute : ModuleAttribute, IOwinModuleMetadata
    {
        /// <summary>
        ///     Default constructor calls the ModuleAttribute with the interface type so it can be imported accordingly
        /// </summary>
        public OwinModuleAttribute() : base(typeof (IOwinModule))
        {
        }

        /// <summary>
        ///     Use a specific contract name for the IOwinConfigure
        ///     Calls the ModuleAttribute constructor with contractName and the interface type so it can be imported accordingly
        /// </summary>
        /// <param name="contractName"></param>
        public OwinModuleAttribute(string contractName) : base(contractName, typeof (IOwinModule))
        {
        }

        /// <summary>
        ///     Here the order of the IOwinModule Initialize can be specified, starting with low values and ending with high.
        /// </summary>
        public int StartupOrder { get; set; } = (int)OwinModuleStartupOrders.User;

        /// <summary>
        ///     Here the order of the IOwinModule Deinitialize can be specified, starting with low values and ending with high.
        /// </summary>
        public int ShutdownOrder { get; set;  } = (int)OwinModuleStartupOrders.User;
    }
}