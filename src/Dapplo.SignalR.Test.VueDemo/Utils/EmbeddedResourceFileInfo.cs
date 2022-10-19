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

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Owin.FileSystems;

namespace Dapplo.SignalR.Test.VueDemo.Utils;

/// <summary>
/// This returns the file info on embedded resources
/// </summary>
public class EmbeddedResourceFileInfo : IFileInfo
{
    private readonly Assembly _assembly;
    private readonly IEnumerable<IFileInfoStreamModifier> _fileInfoStreamModifiers;
    private readonly string _resourcePath;

    private long? _length;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="assembly">Assembly</param>
    /// <param name="resource">Tuple</param>
    /// <param name="lastModified">DateTime</param>
    /// <param name="fileInfoStreamModifiers">IEnumerable of IFileInfoStreamModifier</param>
    public EmbeddedResourceFileInfo(Assembly assembly, Tuple<string, string> resource, DateTime lastModified, IEnumerable<IFileInfoStreamModifier> fileInfoStreamModifiers = null)
    {
        _assembly = assembly;
        _fileInfoStreamModifiers = fileInfoStreamModifiers;
        LastModified = lastModified;
        _resourcePath = resource.Item1;
        Name = resource.Item2;
    }

    /// <summary>
    /// Length of the resource
    /// </summary>
    public long Length
    {
        get
        {
            if (_length.HasValue)
            {
                return _length.Value;
            }

            using (var stream = _assembly.GetManifestResourceStream(_resourcePath))
            {
                _length = stream?.Length ?? 0;
            }

            return _length.Value;
        }
    }

    /// <summary>
    /// As this is an embedded Resource, the file does not have a PhysicalPath
    /// </summary>
    public string PhysicalPath => null;

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public DateTime LastModified { get; }

    /// <summary>
    ///  As this is an embedded Resource, there is no directory
    /// </summary>
    public bool IsDirectory => false;

    /// <summary>
    /// Return the read stream of the resource
    /// </summary>
    /// <returns>Stream</returns>
    public Stream CreateReadStream()
    {
        var stream = _assembly.GetManifestResourceStream(_resourcePath);
        if (!_length.HasValue)
        {
            _length = stream?.Length ?? 0;
        }

        if (_fileInfoStreamModifiers != null)
        {
            foreach (var fileInfoStreamModifier in _fileInfoStreamModifiers)
            {
                if (!fileInfoStreamModifier.CanModifyStream(this))
                {
                    continue;
                }

                stream = fileInfoStreamModifier.ModifyStream(this, stream);
            }
        }
        return stream;
    }
}