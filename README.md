Dapplo.Owin
=====================
Work in progress

- Current build status: [![Build status](https://ci.appveyor.com/api/projects/status/udg7uc9bdr9ps41p?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-owin)
- Coverage: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.Owin/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.Owin?branch=master)
- NuGet package: [![NuGet package](https://badge.fury.io/nu/Dapplo.Owin.svg)](https://badge.fury.io/nu/Dapplo.Owin)

A Dapplo module to "bootstrap" Owin and have modules adding to the configuration instead of a single "Startup" class.

Example:

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
