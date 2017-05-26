//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2017 Dapplo
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

using Dapplo.Log;
using System.ComponentModel.Composition;
using Dapplo.SignalR.Tests.Configuration;
using Xunit;
using Microsoft.AspNet.SignalR;

namespace Dapplo.SignalR.Tests.Hub
{
	/// <summary>
	///     The share context hub
	/// </summary>
	public class TestHub : Hub<ITestHubClient>, ITestHubServer
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		private IMyTestConfiguration MyTestConfiguration { get; set; }

	    /// <inheritdoc />
	    public string Hello(string testString)
		{
			Assert.NotNull(MyTestConfiguration);
		    var returnValue = $"Hello {testString}";

            Log.Verbose().WriteLine(returnValue);
			Clients.Others.TestCalled(testString);
		    return returnValue;

		}
	}
}
