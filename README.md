Dapplo.Owin
=====================
Work in progress

- Current build status: [![Build status](https://ci.appveyor.com/api/projects/status/udg7uc9bdr9ps41p?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-owin)
- Coverage: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.Owin/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.Owin?branch=master)
- NuGet package: [![NuGet package](https://badge.fury.io/nu/Dapplo.Owin.svg)](https://badge.fury.io/nu/Dapplo.Owin)

Dapplo modules to "bootstrap" Owin and SignalR have modules adding to the configuration instead of a single "Startup" class.


An example to use Owin
´´´
	[OwinStartup]
	public class OwinStartupTest : IOwinStartup
	{
		public void Configuration(IOwinServer server, IAppBuilder appBuilder)
		{
			appBuilder.Use(async (owinContext, next) =>
			{
				owinContext.Response.StatusCode = 200;
				owinContext.Response.ContentType = "text/plain";
				await owinContext.Response.WriteAsync("Dapplo");
				await next();
			});
		}
	}
´´´

To use SignalR you will need to add Dapplo.SignalR to your Project and make sure the Dapplo Bootstrapper loads the right DLL's, more later.