Dapplo.Owin
=====================
Work in progress

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
