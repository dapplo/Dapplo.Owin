using System.ComponentModel;

namespace Dapplo.Owin
{
	/// <summary>
	/// Meta-data belonging to the OwinStartupAttribute, which makes it possible to specify type-safe meta-data.
	/// </summary>
	public interface IOwinStartupMetadata
	{
		[DefaultValue(1)]
		int StartupOrder
		{
			get;
		}

	}
}
