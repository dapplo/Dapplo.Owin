﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;

#endregion

namespace Dapplo.Owin
{
	/// <summary>
	/// Helper to start/stop the owin server, extend this class and add the
	/// StartupAction / ShutdownAction attributues... and you are set.
	/// </summary>
	public class SimpleOwinStartStopAction : IAsyncStartupAction, IAsyncShutdownAction
	{
		[Import]
		private IOwinServer OwinServer { get; set; }

		/// <summary>
		/// This starts Owin
		/// </summary>
		/// <param name="token">CancellationToken</param>
		public Task StartAsync(CancellationToken token = new CancellationToken())
		{
			return OwinServer.StartAsync(token);
		}

		/// <summary>
		/// This stops Owin
		/// </summary>
		/// <param name="token">CancellationToken</param>
		public Task ShutdownAsync(CancellationToken token = new CancellationToken())
		{
			return OwinServer.ShutdownAsync(token);
		}
	}
}