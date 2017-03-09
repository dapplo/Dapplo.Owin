using System.ComponentModel;
using Dapplo.Ini;

namespace Dapplo.SignalR
{
	/// <summary>
	/// Settings for the SignalR configuration
	/// </summary>
	public interface ISignalRConfiguration : IIniSubSection
	{
		/// <summary>
		/// Allows to control the generation of Signal-R JavaScript proxies
		/// </summary>
		[Description("Enable the generation of SignalR JavaScript proxies")]
		[DefaultValue(true)]
		bool EnableJavaEnableJavaScriptProxies { get; set; }

		/// <summary>
		/// Enable detailed error information for SignalR
		/// </summary>
		[Description("Enable detailed error information for SignalR")]
		[DefaultValue(true)]
		bool EnableDetailedErrors { get; set; }

		/// <summary>
		/// Enable detailed error information for SignalR
		/// </summary>
		[Description("Fix camel casing for SignalR")]
		[DefaultValue(true)]
		bool FixCamelCase { get; set; }
	}
}
