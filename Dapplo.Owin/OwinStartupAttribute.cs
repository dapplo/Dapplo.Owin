using Dapplo.Addons;
using System;
using System.ComponentModel.Composition;

namespace Dapplo.Owin
{
	[MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class OwinStartupAttribute : ModuleAttribute, IOwinStartupMetadata
	{
		public OwinStartupAttribute() : base(typeof(IOwinStartup))
		{

		}

		/// <summary>
		/// Use a specific contract name for the IOwinStartup
		/// </summary>
		/// <param name="contractName"></param>
		public OwinStartupAttribute(string contractName) : base(contractName, typeof(IOwinStartup))
		{

		}

		/// <summary>
		/// Here the order of the startup action can be specified, starting with low values and ending with high.
		/// With this a cheap form of "dependency" management is made.
		/// </summary>
		public int StartupOrder
		{
			get;
			set;
		} = 1;
	}
}
