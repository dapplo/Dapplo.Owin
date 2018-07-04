//  Dapplo - building blocks for desktop applications
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

using System.Net;
using Dapplo.Addons;
using Dapplo.Log;
using Owin;

namespace Dapplo.Owin.OwinModules
{
	/// <summary>
	///     An Owin Module which configures the Authentication
	/// </summary>
	[Service(nameof(ConfigureOwinAuthentication))]
	public class ConfigureOwinAuthentication : BaseOwinModule
	{
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		///     Configure the authentication scheme for Owin
		/// </summary>
		/// <param name="server">IOwinServer</param>
		/// <param name="appBuilder">IAppBuilder</param>
		public override void Configure(IOwinServer server, IAppBuilder appBuilder)
		{
			Log.Verbose().WriteLine("AuthenticationScheme is configured to {0}", server.OwinConfiguration.AuthenticationScheme);
			// Enable Authentication, if a scheme is set
			if (server.OwinConfiguration.AuthenticationScheme == AuthenticationSchemes.None)
			{
				return;
			}

			var propertyName = typeof(HttpListener).FullName;
			if (propertyName == null)
			{
				return;
			}
            var listener = (HttpListener)appBuilder.Properties[propertyName];
			listener.AuthenticationSchemes = server.OwinConfiguration.AuthenticationScheme;
		}
	}
}
