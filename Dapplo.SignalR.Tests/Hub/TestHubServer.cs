using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapplo.SignalR.Tests.Hub
{
	public interface TestHubServer
	{
		void Test(string testString);
	}
}
