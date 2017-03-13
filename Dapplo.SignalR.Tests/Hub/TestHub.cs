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
	public class TestHub : Hub<TestHubClient>, TestHubServer
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		private IMyTestConfiguration MyTestConfiguration { get; set; }

		/// <summary>
		/// This method is offered as a service to the client and will share a context
		/// </summary>
		/// <param name="testString">some string to test with</param>
		public void Test(string testString)
		{
			Assert.NotNull(MyTestConfiguration);
			Log.Verbose().WriteLine("Teststring {0}", testString);
			Clients.Others.TestCalled(testString);
		}
	}
}
