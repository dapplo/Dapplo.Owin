//  Dapplo - building blocks for desktop applications
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
using System.Net;
using System.Runtime.Serialization;
using Dapplo.Ini;

#endregion

namespace Dapplo.Owin
{
	/// <summary>
	///     The Owin configuration container, this can be stored with Dapplo.Config
	/// </summary>
	public interface IOwinConfiguration : IIniSubSection
	{
		/// <summary>
		/// Hostname for Owin to accept request on, default is localhost
		/// </summary>
		[DefaultValue("localhost"), Description("Host for Owin to accept request on."), DataMember(EmitDefaultValue = true)]
		string Hostname { get; set; }

		/// <summary>
		/// Port for owin to accept requests on, default is 8080
		/// </summary>
		[DefaultValue(8080), Description("Port for Owin to accept request on."), DataMember(EmitDefaultValue = true)]
		int Port { get; set; }


		/// <summary>
		/// Enable serving of files from a html sub folder, this can be used to allow error pages.
		/// </summary>
		[Description("Enable serving of files from a html sub folder, this can be used to allow error pages"), DefaultValue(true)]
		bool EnableFileServer { get; set; }

		/// <summary>
		/// Specify if an error page should be shown
		/// </summary>
		[Description("Show an error page when something happens"), DefaultValue(false)]
		bool UseErrorPage { get; set; }

		/// <summary>
		/// Enable Cross Origin calls
		/// </summary>
		[Description("Set this to true to allow cross origin calls, this is needed when your html page is not served by the owin server.")]
		[DefaultValue(true)]
		bool EnableCors { get; set; }

		/// <summary>
		/// Specify what AuthenticationScheme is used, default is none
		/// </summary>
		[Description("The Authentication scheme for Owin"), DefaultValue(AuthenticationSchemes.None)]
		AuthenticationSchemes AuthenticationScheme { get; set; }
	}
}