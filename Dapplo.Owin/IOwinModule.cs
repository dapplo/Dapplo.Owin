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

#region using

using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Owin;

#endregion

namespace Dapplo.Owin
{
	/// <summary>
	///     The IOwinStartup describes Owin modules that add functionality to an Owin server, like SignalR or a file-servers
	/// </summary>
	public interface IOwinModule
	{
		/// <summary>
		/// If you need to initialize something, do it here
		/// </summary>
		/// <param name="server">IOwinServer</param>
		/// <param name="cancellationToken">CancellationToken</param>
		Task InitializeAsync(IOwinServer server, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Implement this to make sure you can configure the pipeline, like add a middleware
		/// </summary>
		/// <param name="server">IOwinServer</param>
		/// <param name="appBuilder">IAppBuilder</param>
		void Configure(IOwinServer server, IAppBuilder appBuilder);

		/// <summary>
		/// Optionally implement code here to make sure you shutdown any running services or cleanup resources.
		/// </summary>
		/// <param name="server">IOwinServer</param>
		/// <param name="cancellationToken">CancellationToken</param>
		Task DeinitializeAsync(IOwinServer server, CancellationToken cancellationToken = default(CancellationToken));
	}
}