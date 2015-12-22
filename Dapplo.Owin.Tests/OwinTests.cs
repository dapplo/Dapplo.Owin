using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Dapplo.Addons.Implementation;
using Dapplo.Config.Ini;
using Dapplo.HttpExtensions;

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
			var owinConfig = await iniConfig.RegisterAndGetAsync<IOwinConfiguration>();

			bootstrapper.Add(typeof(OwinStartupTest));
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

			// Test request
			var testUri = new Uri($"http://{owinConfig.Host}:{owinConfig.Port}/Test");
			var result = await testUri.GetAsStringAsync();
			Assert.AreEqual("Dapplo", result);

			// Test shutdown
			await bootstrapper.ShutdownAsync();
		}
	}
}
