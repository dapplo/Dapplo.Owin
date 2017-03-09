using System;
using System.Reflection;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace Dapplo.SignalR
{
	/// <summary>
	///     This solves the problem that signalr communication is made with PascalCase instead of camelCase
	///     For more information, see <a href="http://stackoverflow.com/a/30019100/1886251">here</a>
	/// </summary>
	public class SignalRContractResolver : IContractResolver
	{
		private readonly Assembly _assembly;
		private readonly IContractResolver _camelCaseContractResolver;
		private readonly IContractResolver _defaultContractSerializer;

		/// <summary>
		///     Constructor
		/// </summary>
		public SignalRContractResolver()
		{
			_defaultContractSerializer = new DefaultContractResolver();
			_camelCaseContractResolver = new CamelCaseContractResolver();
			_assembly = typeof(Connection).Assembly;
		}

		JsonContract IContractResolver.ResolveContract(Type type)
		{
			if (type.Assembly.Equals(_assembly))
			{
				return _defaultContractSerializer.ResolveContract(type);
			}

			return _camelCaseContractResolver.ResolveContract(type);
		}
	}
}
