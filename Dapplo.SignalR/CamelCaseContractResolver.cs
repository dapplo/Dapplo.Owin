using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dapplo.SignalR
{
	/// <summary>
	/// This is a helper class for the Json resolver, which helps to format things as they should be in JavaScript.
	/// It will exclude IObservables, as these are not serializable.
	/// </summary>
	public class CamelCaseContractResolver : CamelCasePropertyNamesContractResolver
	{
		/// <summary>
		///     Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given
		///     <see cref="T:System.Reflection.MemberInfo" />.
		/// Excludes IObservables.
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
			if (typeof(IObservable<>).IsAssignableFrom(property.DeclaringType))
			{
				property.ShouldSerialize = instance => false;
			}
			return property;
		}
	}

}
