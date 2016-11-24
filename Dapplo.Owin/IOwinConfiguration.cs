﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
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

#region using

using System.ComponentModel;
using System.Runtime.Serialization;
using Dapplo.Config.Ini;

#endregion

namespace Dapplo.Owin
{
	/// <summary>
	///     The Owin configuration container, this can be stored with Dapplo.Config
	/// </summary>
	public interface IOwinConfiguration : IIniSubSection
	{
		[DefaultValue("localhost"), Description("Host for Owin to accept request on."), DataMember(EmitDefaultValue = true)]
		string Hostname { get; set; }

		[DefaultValue(8080), Description("Port for Owin to accept request on."), DataMember(EmitDefaultValue = true)]
		int Port { get; set; }
	}
}