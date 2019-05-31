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

using System.Threading.Tasks;
using Dapplo.Log;
using Microsoft.Owin;

namespace Dapplo.Owin.Tests.Owin
{
    /// <summary>
    ///     The "test" Middleware, it returns "Dapplo" for EVERY request
    /// </summary>
    public class TestMiddleware : OwinMiddleware
    {
        private static readonly LogSource Log = new LogSource();

        public TestMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext owinContext)
        {
            Log.Debug().WriteLine("Http method: {0}, path: {1}", owinContext.Request.Method, owinContext.Request.Path);
            var user = owinContext.Authentication?.User;
            Log.Debug().WriteLine("User: {0}", user?.Identity?.Name ?? "not available");
            owinContext.Response.StatusCode = 200;
            owinContext.Response.ContentType = "text/plain";
            await owinContext.Response.WriteAsync("Dapplo").ConfigureAwait(false);
            await Next.Invoke(owinContext).ConfigureAwait(false);
        }
    }
}