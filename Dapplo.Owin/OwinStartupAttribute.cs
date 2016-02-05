﻿/*
	Dapplo - building blocks for desktop applications
	Copyright (C) 2015-2016 Dapplo

	For more information see: http://dapplo.net/
	Dapplo repositories are hosted on GitHub: https://github.com/dapplo

	This file is part of Dapplo.Owin.

	Dapplo.Owin is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Dapplo.Owin is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Dapplo.Owin. If not, see <http://www.gnu.org/licenses/>.
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
