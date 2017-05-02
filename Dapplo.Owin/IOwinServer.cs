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
using Dapplo.Addons;
using Dapplo.Owin.Configuration;

#endregion

namespace Dapplo.Owin
{
    /// <summary>
    ///     The IOwinServer is the public interface for the "server".
    ///     This is also available via the Dapplo.Addons IServiceLocator, and can be imported
    /// </summary>
    public interface IOwinServer : IAsyncStartupAction, IAsyncShutdownAction
    {
        /// <summary>
        /// The Owin Server configuration
        /// </summary>
        IOwinConfiguration OwinConfiguration { get; }

        /// <summary>
        ///     Is the server running?
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        ///     The Uri where Owin is listening on
        /// </summary>
        Uri ListeningOn { get; }
    }
}