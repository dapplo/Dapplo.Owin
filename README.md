# Dapplo.Owin
A Dapplo module to "bootstrap" Owin and have modules adding to the configuration instead of a single "Startup" class.

Example:

	[OwinStartup]
	public class OwinStartupTest : IOwinStartup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			appBuilder.Use(async (owinContext, next) =>
			{
				await owinContext.Response.WriteAsync("Dapplo");
				owinContext.Response.StatusCode = 200;
				await next();
			});
		}
	}
