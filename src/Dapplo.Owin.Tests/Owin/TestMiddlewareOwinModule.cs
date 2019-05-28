//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Owin
// 
//  Dapplo.Owin is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Owin is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Owin. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using Dapplo.Addons;
using Dapplo.Log;
using Dapplo.Owin.Tests.Configuration;
using Owin;

namespace Dapplo.Owin.Tests.Owin
{
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
}