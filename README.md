Dapplo.Owin
=====================
Work in progress

- Current build status: [![Build status](https://ci.appveyor.com/api/projects/status/udg7uc9bdr9ps41p?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-owin)
- Coverage: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.Owin/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.Owin?branch=master)
- NuGet package: [![NuGet package](https://badge.fury.io/nu/Dapplo.Owin.svg)](https://badge.fury.io/nu/Dapplo.Owin)

This makes it possible to "bootstrap" Owin and SignalR, have modules adding to the configuration instead of a single "Startup" class.
As Dapplo.Owin is based on Dapplo.Addons, you can use existing Autofac knowledge.

An example OwinModule
´´´
	/// <summary>
    /// A simple Owin module
    /// </summary>
	[Service(nameof(TestMiddlewareOwinModule))]
    public class TestMiddlewareOwinModule : BaseOwinModule
	{
		private static readonly LogSource Log = new LogSource();

        /// <summary>
        /// Constructor which can take dependencies
        /// </summary>
        /// <param name="myTestConfiguration">IMyTestConfiguration</param>
        // ReSharper disable once UnusedParameter.Local
        public TestMiddlewareOwinModule(IMyTestConfiguration myTestConfiguration)
		{
			// do something with your configuration
		}

        /// <summary>
        /// Configure the IAppBuilder
        /// </summary>
        /// <param name="server">IOwinServer</param>
        /// <param name="appBuilder">IAppBuilder</param>
        public override void Configure(IOwinServer server, IAppBuilder appBuilder)
		{
			Log.Debug().WriteLine("Configuring test middleware in the Owin pipeline");
			appBuilder.Use(typeof (TestMiddleware));
		}
	}
´´´

To use SignalR you will need to add Dapplo.SignalR to your Project and make sure the Dapplo Bootstrapper loads the right DLL's, more later.
