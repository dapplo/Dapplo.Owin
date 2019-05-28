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

using Dapplo.Addons;
using Dapplo.Log;
using Owin;

namespace Dapplo.Owin.OwinModules
{
    /// <summary>
    ///     An Owin Module which configures the error page
    /// </summary>
    [Service(nameof(ConfigureOwinErrorPage))]
    public class ConfigureOwinErrorPage : BaseOwinModule
	{
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		///     Configure the error page for Owin
		/// </summary>
		/// <param name="server">IOwinServer</param>
		/// <param name="appBuilder">IAppBuilder</param>
		public override void Configure(IOwinServer server, IAppBuilder appBuilder)
		{
			Log.Verbose().WriteLine("Enabling error page: {0}", server.OwinConfiguration.UseErrorPage);
			if (!server.OwinConfiguration.UseErrorPage)
			{
				return;
			}
			appBuilder.UseErrorPage();
		}
	}
}
