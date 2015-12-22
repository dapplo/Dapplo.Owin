using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Dapplo.Addons.Implementation;
using Dapplo.Config.Ini;

namespace Dapplo.Owin.Tests
{
	[TestClass]
	public class OwinTests
	{
		private const string ApplicationName = "DapploOwin";
		[TestMethod]
		public async Task TestStartup()
		{
			var bootstrapper = new ApplicationBootstrapper(ApplicationName);
			var iniConfig = new IniConfig(ApplicationName, "test");
			bootstrapper.IniConfig = iniConfig;

			bootstrapper.Add(".", "Dapplo.*.dll");
			// Add test project, without having a direct reference
#if DEBUG
			bootstrapper.Add(@"..\..\..\Dapplo.Owin\bin\Debug", "Dapplo.*.dll");
#else
			bootstrapper.Add(@"..\..\..\Dapplo.Owin\bin\Release", "Dapplo.*.dll");
#endif
			// Initialize, so we can export
			bootstrapper.Initialize();

			// Start the composition
			bootstrapper.Run();
			// Test startup
			await bootstrapper.StartupAsync();

			// Test shutdown
			await bootstrapper.ShutdownAsync();
		}
	}
}
