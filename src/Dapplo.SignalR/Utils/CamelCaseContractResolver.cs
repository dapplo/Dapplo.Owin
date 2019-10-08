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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dapplo.SignalR.Utils
{
	/// <summary>
	///     This is a helper class for the Json resolver, which helps to format things as they should be in JavaScript.
	///     It will exclude IObservables, as these are not serializable.
	/// </summary>
	internal class CamelCaseContractResolver : CamelCasePropertyNamesContractResolver
	{
		/// <summary>
		///     Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given
		///     <see cref="T:System.Reflection.MemberInfo" />.
		///     Excludes IObservables.
		/// </summary>
		/// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
		/// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
		/// <returns>
		///     A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given
		///     <see cref="T:System.Reflection.MemberInfo" />.
		/// </returns>
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var property = base.CreateProperty(member, memberSerialization);
			if (property.PropertyType.IsGenericType && typeof(IObservable<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()))
			{
				property.ShouldSerialize = instance => false;
			}
			return property;
		}
	}
}