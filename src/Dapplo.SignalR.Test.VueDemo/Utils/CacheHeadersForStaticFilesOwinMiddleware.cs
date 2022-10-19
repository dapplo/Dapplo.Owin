//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2022 Dapplo
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

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Dapplo.SignalR.Test.VueDemo.Utils;

/// <inheritdoc />
public class CacheHeadersForStaticFilesOwinMiddleware : OwinMiddleware
{
    private static readonly Regex StaticFilesRegex = new Regex(@".*(\.js|\.ts|\.map|\.html|\.css)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    /// <inheritdoc />
    public CacheHeadersForStaticFilesOwinMiddleware(OwinMiddleware next) : base(next)
    {
    }

    /// <inheritdoc />
    public override Task Invoke(IOwinContext context)
    {
        if (!context.Request.Path.HasValue)
        {
            return Next.Invoke(context);
        }

        var path = context.Request.Path.Value;
        if (StaticFilesRegex.IsMatch(path))
        {
            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
        }
        else
        {
            // Keep files max 1 working day (9 hours)
            context.Response.Headers.Add("Cache-Control", new[] { "public", "max-age=32400" });
        }
        return Next.Invoke(context);
    }
}