/*
 * dapplo - building blocks for desktop applications
 * Copyright (C) Dapplo 2015-2016
 * 
 * For more information see: http://dapplo.net/
 * dapplo repositories are hosted on GitHub: https://github.com/dapplo
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using Dapplo.Addons.Implementation;
using Dapplo.Config.Ini;
using Dapplo.HttpExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
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
			var testUri = new Uri($"http://{owinConfig.Hostname}:{owinConfig.Port}/Test");
			var result = await testUri.GetAsStringAsync();
			Assert.AreEqual("Dapplo", result);

			// Test shutdown
			await bootstrapper.ShutdownAsync();
		}
	}
}
