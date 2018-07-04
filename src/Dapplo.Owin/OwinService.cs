//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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

using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;

#endregion

namespace Dapplo.Owin
{
    /// <summary>
    /// Basic service implementation to start/stop the owin server, register this with Autofac as  Dapplo.Addons.IService
    /// Or extend to bring your own...
    /// </summary>
    [Service(nameof(OwinService))]
    public class OwinService : IStartupAsync, IShutdownAsync
    {
		/// <summary>
		/// The IOwinServer
		/// </summary>
		protected IOwinServer OwinServer { get;}

	    /// <inheritdoc />
	    public OwinService(IOwinServer owinServer)
	    {
		    OwinServer = owinServer;
	    }

		/// <summary>
		/// This starts Owin
		/// </summary>
		/// <param name="token">CancellationToken</param>
		public virtual Task StartupAsync(CancellationToken token = new CancellationToken())
		{
			return OwinServer.StartupAsync(token);
		}

		/// <summary>
		/// This stops Owin
		/// </summary>
		/// <param name="token">CancellationToken</param>
		public virtual Task ShutdownAsync(CancellationToken token = new CancellationToken())
		{
			return OwinServer.ShutdownAsync(token);
		}
	}
}