﻿#region Dapplo License

//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2017 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.SignalR
// 
//  Dapplo.SignalR is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.SignalR is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.SignalR. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System.ComponentModel;
using System.Runtime.Serialization;
using Dapplo.Ini;

#endregion

namespace Dapplo.SignalR.Configuration
{
	/// <summary>
	///     Settings for the SignalR configuration
	/// </summary>
	public interface ISignalRConfiguration : IIniSubSection
	{
		/// <summary>
		///     Allows to control the generation of Signal-R JavaScript proxies
		/// </summary>
		[Description("Enable the generation of SignalR JavaScript proxies")]
		[DefaultValue(true)]
		bool EnableJavaEnableJavaScriptProxies { get; set; }

		/// <summary>
		///     Enable detailed error information for SignalR
		/// </summary>
		[Description("Enable detailed error information for SignalR")]
		[DefaultValue(true)]
		[DataMember(EmitDefaultValue = true)]
		bool EnableDetailedErrors { get; set; }

		/// <summary>
		///     Enable detailed error information for SignalR
		/// </summary>
		[Description("Fix camel casing for SignalR")]
		[DefaultValue(true)]
		bool FixCamelCase { get; set; }

		/// <summary>
		///     Using a default performance counter can solve some known issues
		/// </summary>
		[Description("Use a dummy performance counter to solve some issues.")]
		[DefaultValue(true), DataMember(EmitDefaultValue = false)]
		bool UseDummyPerformanceCounter { get; set; }
	}
}