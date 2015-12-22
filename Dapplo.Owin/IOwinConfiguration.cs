using Dapplo.Config.Ini;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Dapplo.Owin
{
	[IniSection("Owin")]
	public interface IOwinConfiguration : IIniSection
	{
		[DefaultValue(8080), Description("Port for Owin to accept request on."), DataMember(EmitDefaultValue = true)]
		int Port
		{
			get;
			set;
		}

		[DefaultValue("localhost"), Description("Host for Owin to accept request on."), DataMember(EmitDefaultValue = true)]
		string Host
		{
			get;
			set;
		}
	}
}
