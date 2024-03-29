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

using System.Threading.Tasks;
using Dapplo.SignalR.Test.VueDemo.Model;

namespace Dapplo.SignalR.Test.VueDemo.Hubs;

/// <summary>
/// Interface for the server
/// </summary>
public interface IVueHubServer
{
    /// <summary>
    /// Method for the client to call
    /// TODO: Clients can't work with interfaces!
    /// </summary>
    /// <param name="myVueModel">MyVueModel</param>
    Task StoreModelChange(MyVueModel myVueModel);
}