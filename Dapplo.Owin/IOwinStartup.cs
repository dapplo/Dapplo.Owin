using Dapplo.Addons;
using Owin;

namespace Dapplo.Owin
{
	/// <summary>
	/// The IOwinStartup describes Owin modules that add functionality to an Owin server, like SignalR or a file-servers
	/// </summary>
	public interface IOwinStartup : IModule
	{
		void Configuration(IAppBuilder appBuilder);
	}
}
