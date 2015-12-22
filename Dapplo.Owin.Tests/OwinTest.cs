using Owin;
using System;

namespace Dapplo.Owin.Tests
{
	[OwinStartup]
	public class OwinTest : IOwinStartup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			throw new NotImplementedException();
		}
	}
}
