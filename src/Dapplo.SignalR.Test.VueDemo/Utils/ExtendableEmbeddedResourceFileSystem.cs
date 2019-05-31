//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2019 Dapplo
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
using System.Linq;
using System.Reflection;
using Microsoft.Owin.FileSystems;

namespace Dapplo.SignalR.Test.VueDemo.Utils
{
    /// <summary>
    /// This is a workaround for the EmbeddedResourceFileSystem which uses Assembly.Location to get the timestamp of the file
    /// </summary>
    public class ExtendableEmbeddedResourceFileSystem : IFileSystem
    {
        private readonly Func<Assembly, Tuple<string, string>, DateTime, IFileInfo> _fileInfoFactory;
        private readonly Assembly _assembly;
        private readonly string _baseNamespace;
        private readonly DateTime _lastModified;
        private readonly string[] _manifestResourceNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableEmbeddedResourceFileSystem" /> class using the calling
        /// assembly and empty base namespace.
        /// </summary>
        public ExtendableEmbeddedResourceFileSystem(Func<Assembly, Tuple<string, string>, DateTime, EmbeddedResourceFileInfo> fileInfoFactory)
            : this(fileInfoFactory, Assembly.GetCallingAssembly())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableEmbeddedResourceFileSystem" /> class using the specified
        /// assembly and empty base namespace.
        /// </summary>
        /// <param name="fileInfoFactory">Func for factory</param>
        /// <param name="assembly">Assembly</param>
        public ExtendableEmbeddedResourceFileSystem(Func<Assembly, Tuple<string, string>, DateTime, EmbeddedResourceFileInfo> fileInfoFactory, Assembly assembly)
            : this(fileInfoFactory, assembly, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableEmbeddedResourceFileSystem" /> class using the calling
        /// assembly and specified base namespace.
        /// </summary>
        /// <param name="fileInfoFactory">Func for factory</param>
        /// <param name="baseNamespace">The base namespace that contains the embedded resources.</param>
        public ExtendableEmbeddedResourceFileSystem(Func<Assembly, Tuple<string, string>, DateTime, EmbeddedResourceFileInfo> fileInfoFactory, string baseNamespace)
            : this(fileInfoFactory, Assembly.GetCallingAssembly(), baseNamespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableEmbeddedResourceFileSystem" /> class using the specified
        /// assembly and base namespace.
        /// </summary>
        /// <param name="fileInfoFactory">Func for factory</param>
        /// <param name="assembly">The assembly that contains the embedded resources.</param>
        /// <param name="baseNamespace">The base namespace that contains the embedded resources.</param>
        public ExtendableEmbeddedResourceFileSystem(Func<Assembly, Tuple<string, string>, DateTime, EmbeddedResourceFileInfo> fileInfoFactory, Assembly assembly, string baseNamespace)
        {
            _fileInfoFactory = fileInfoFactory;
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _baseNamespace = string.IsNullOrEmpty(baseNamespace) ? string.Empty : baseNamespace + ".";
            var location = string.IsNullOrEmpty(assembly.Location) ? new Uri(assembly.CodeBase).LocalPath : assembly.Location;
            _lastModified = new FileInfo(location).LastWriteTime;
            _manifestResourceNames = _assembly.GetManifestResourceNames();
        }

        /// <summary>
        /// Locate a file at the given path
        /// </summary>
        /// <param name="subPath">The path that identifies the file</param>
        /// <param name="fileInfo">The discovered file if any</param>
        /// <returns>True if a file was located at the given path</returns>
        public bool TryGetFileInfo(string subPath, out IFileInfo fileInfo)
        {
            // "/file.txt" expected.
            if (string.IsNullOrEmpty(subPath) || subPath[0] != '/')
            {
                fileInfo = null;
                return false;
            }

            var fileName = subPath.Substring(1); // Drop the leading '/'
            var resourcePath = _baseNamespace + fileName.Replace('/', '.');

            if (_assembly.GetManifestResourceInfo(resourcePath) == null)
            {
                fileInfo = null;
                return false;
            }
            fileInfo = _fileInfoFactory(_assembly, new Tuple<string, string>(resourcePath, fileName), _lastModified);
            return true;
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// This file system uses a flat directory structure. Everything under the base namespace is considered to be one directory.
        /// </summary>
        /// <param name="subPath">The path that identifies the directory</param>
        /// <param name="contents">The contents if any</param>
        /// <returns>True if a directory was located at the given path</returns>
        public bool TryGetDirectoryContents(string subPath, out IEnumerable<IFileInfo> contents)
        {
            // The file name is assumed to be the remainder of the resource name.

            // Non-hierarchical.
            if (!subPath.Equals("/"))
            {
                contents = null;
                return false;
            }

            contents = _manifestResourceNames
                .Where(resourceName => resourceName.StartsWith(_baseNamespace))
                .Select(resourceName => _fileInfoFactory(_assembly, new Tuple<string, string>(resourceName, resourceName.Substring(_baseNamespace.Length)), _lastModified));
            return true;
        }
    }
}
