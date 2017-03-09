﻿using System.ComponentModel.Composition;
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
	///     A generic Owin initialization
	/// </summary>
	public abstract class DefaultOwinModule : SimpleOwinModule
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
			if (OwinConfiguration.UseErrorPage)
			{
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

			if (OwinConfiguration.EnableFileServer)
			{
				//// Add the file server for the error pages
				appBuilder.UseFileServer(new FileServerOptions
				{
					EnableDefaultFiles = true,
					RequestPath = new PathString("/html"),
					FileSystem = new PhysicalFileSystem("Html"),
					EnableDirectoryBrowsing = false
				});
			}

			if (OwinConfiguration.EnableCors)
			{
				// Enable CORS (allow cross domain requests)
				appBuilder.UseCors(CorsOptions.AllowAll);
			}
		}
	}
}
