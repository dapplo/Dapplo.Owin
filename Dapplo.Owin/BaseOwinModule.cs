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
using Owin;

#endregion

namespace Dapplo.Owin
{
	/// <summary>
	/// A base implementation for an Owin module, don't forget to mark you
	/// </summary>
	public abstract class BaseOwinModule : IOwinModule
	{
		/// <summary>
		/// This is a default implementation, which does nothing
		/// </summary>
		public virtual Task InitializeAsync(IOwinServer server, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.FromResult(true);
		}

		/// <inheritdoc />
		public abstract void Configure(IOwinServer server, IAppBuilder appBuilder);

		/// <inheritdoc />
		/// <summary>
		/// A default implementation, which does nothing
		/// </summary>
		public virtual Task DeinitializeAsync(IOwinServer server, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.FromResult(true);
		}
	}
}