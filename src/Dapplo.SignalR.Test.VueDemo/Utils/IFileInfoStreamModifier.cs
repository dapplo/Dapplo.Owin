//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2022 Dapplo
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

using System.IO;
using Microsoft.Owin.FileSystems;

namespace Dapplo.SignalR.Test.VueDemo.Utils;

/// <summary>
/// 
/// </summary>
public interface IFileInfoStreamModifier
{
    /// <summary>
    /// This tells the EmbeddedResourceFileInfo if the stream can be modified
    /// </summary>
    /// <param name="fileInfo">IFileInfo</param>
    /// <returns>bool</returns>
    bool CanModifyStream(IFileInfo fileInfo);

    /// <summary>
    /// This modifies the original stream
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="stream">Stream to modify (or the original if not)</param>
    /// <returns>Stream</returns>
    Stream ModifyStream(IFileInfo fileInfo, Stream stream);
}