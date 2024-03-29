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

using Dapplo.Addons;
using Dapplo.Log;
using Microsoft.Owin.Cors;
using Owin;

namespace Dapplo.Owin.OwinModules;

/// <summary>
///     An Owin Module which configures CORS
/// </summary>
[Service(nameof(ConfigureOwinCors))]
public class ConfigureOwinCors : BaseOwinModule
{
	private static readonly LogSource Log = new LogSource();

	/// <summary>
	///     Configure CORS for Owin
	/// </summary>
	/// <param name="server">IOwinServer</param>
	/// <param name="appBuilder">IAppBuilder</param>
	public override void Configure(IOwinServer server, IAppBuilder appBuilder)
	{
		if (!server.OwinConfiguration.EnableCors)
		{
			Log.Verbose().WriteLine("Cors is disabled.");
			return;
		}
		Log.Verbose().WriteLine("Enabling Cors");
		// Enable CORS (allow cross domain requests)
		appBuilder.UseCors(CorsOptions.AllowAll);
	}
}