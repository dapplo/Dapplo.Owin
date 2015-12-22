using Owin;
using System;
using System.Diagnostics;

namespace Dapplo.Owin.Tests
{
	[OwinStartup]
	public class OwinStartupTest : IOwinStartup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			appBuilder.Use(async (owinContext, next) =>
			{
				Debug.WriteLine($"Http method: {owinContext.Request.Method}, path: {owinContext.Request.Path}");
				await owinContext.Response.WriteAsync("Dapplo");
				owinContext.Response.StatusCode = 200;
				await next();
			});
		}
	}
}
