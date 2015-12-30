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

using Dapplo.Addons;
using System;
using System.ComponentModel.Composition;

namespace Dapplo.Owin
{
	[MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
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
