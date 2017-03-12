using System.ComponentModel.Composition;
using System.Net;
using Dapplo.Addons;
using Dapplo.Log;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Dapplo.Owin
{
	/// <summary>
	///     A Owin Module which configures some sensible defaults, depending on a configuration
	/// </summary>
	public abstract class ConfigureDefaultsOwinModule : SimpleOwinModule
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		private IOwinConfiguration OwinConfiguration { get; set; }

		[Import]
		private IServiceLocator ServiceLocator { get; set; }

		/// <summary>
		///     Configure Owin with a lot of defaults, depending on the values in the OwinConfiguration
		/// </summary>
		/// <param name="server">IOwinServer</param>
		/// <param name="appBuilder">IAppBuilder</param>
		public override void Configure(IOwinServer server, IAppBuilder appBuilder)
		{
			// Enable Authentication, if a scheme is set
			if (OwinConfiguration.AuthenticationScheme != AuthenticationSchemes.None)
			{
				Log.Verbose().WriteLine("Setting AuthenticationScheme to {0}", OwinConfiguration.AuthenticationScheme);
				var listener = (HttpListener)appBuilder.Properties[typeof(HttpListener).FullName];
				listener.AuthenticationSchemes = OwinConfiguration.AuthenticationScheme;
			}

			if (OwinConfiguration.EnableCors)
			{
				Log.Verbose().WriteLine("Enabling Cors");
				// Enable CORS (allow cross domain requests)
				appBuilder.UseCors(CorsOptions.AllowAll);
			}

			if (OwinConfiguration.UseErrorPage)
			{
				Log.Verbose().WriteLine("Using error page");
				appBuilder.UseErrorPage();
			}

			// Handle errors like 404 (not founds)
			appBuilder.Use((owinContext, next) =>
			{
				return next().ContinueWith(x =>
				{
					if (owinContext.Response.StatusCode < 400)
					{
						return;
					}
					var request = owinContext.Request;
					var statusCode = owinContext.Response.StatusCode;
					Log.Warn().WriteLine("{1} -> {0}", statusCode, request.Uri.AbsolutePath);
					owinContext.Response.Redirect(owinContext.Request.Uri.AbsoluteUri.Replace(request.Uri.PathAndQuery, request.PathBase + $"/html/error/{statusCode}.html"));
				});
			});

			if (!OwinConfiguration.EnableFileServer)
			{
				return;
			}
			Log.Verbose().WriteLine("Activating file server");

			//// Add the file server for the error pages
			appBuilder.UseFileServer(new FileServerOptions
			{
				EnableDefaultFiles = true,
				RequestPath = new PathString("/html"),
				FileSystem = new PhysicalFileSystem("Html"),
				EnableDirectoryBrowsing = false
			});
		}
	}
}
