//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2018 Dapplo
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

using System.Collections.Generic;
using Dapplo.Config.Ini;
using System.Net;

namespace Dapplo.Owin.Tests.Configuration
{
    class MyTestConfigurationImpl : IniSectionBase<IMyTestConfiguration>, IMyTestConfiguration
    {
        public IList<string> ListeningUrls { get; set; }
        public bool UseErrorPage { get; set; }
        public bool EnableCors { get; set; }
        public AuthenticationSchemes AuthenticationScheme { get; set; }
    }
}
