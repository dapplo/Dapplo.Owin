//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
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
using System.Threading;
using System.Threading.Tasks;
using Owin;

#endregion

namespace Dapplo.Owin
{
	[OwinModule]
	public abstract class SimpleOwinModule : IOwinModule
	{
		/// <summary>
		/// Do nothing
		/// </summary>
		public Task InitializeAsync(IOwinServer server, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.FromResult(true);
		}

		public abstract void Configure(IOwinServer server, IAppBuilder appBuilder);

		/// <summary>
		/// Do nothing
		/// </summary>
		public Task DeinitializeAsync(IOwinServer server, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.FromResult(true);
		}
	}
}