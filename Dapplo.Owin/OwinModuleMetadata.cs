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

#endregion

namespace Dapplo.Owin
{
	/// <summary>
	/// Meta data for OwinModules, this is for when MEF isn't used.
	/// </summary>
	public class OwinModuleMetadata : IOwinModuleMetadata
	{
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