//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.SignalR
// 
//  Dapplo.SignalR is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.SignalR is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.SignalR. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;
using System.Reflection;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace Dapplo.SignalR.Utils
{
	/// <summary>
	///     This solves the problem that signalR communication is made with PascalCase instead of camelCase
	///     For more information, see <a href="http://stackoverflow.com/a/30019100/1886251">here</a>
	/// </summary>
	internal class SignalRContractResolver : IContractResolver
	{
		private readonly Assembly _assembly;
		private readonly IContractResolver _camelCaseContractResolver;
		private readonly IContractResolver _defaultContractSerializer;

		/// <summary>
		///     Constructor
		/// </summary>
		public SignalRContractResolver(bool fixCamelCase = true)
		{
			_defaultContractSerializer = new DefaultContractResolver();
			if (fixCamelCase)
			{
				_camelCaseContractResolver = new CamelCaseContractResolver();
            }
            _assembly = typeof(Connection).Assembly;
		}

		JsonContract IContractResolver.ResolveContract(Type type)
		{
			if (type.Assembly.Equals(_assembly) || _camelCaseContractResolver is null)
			{
				return _defaultContractSerializer.ResolveContract(type);
			}

			return _camelCaseContractResolver.ResolveContract(type);
		}
	}
}