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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Dapplo.Log;
using Dapplo.Owin.Configuration;

namespace Dapplo.Owin;

/// <summary>
/// Extension method for using the IOwinConfiguration
/// </summary>
public static class OwinConfigurationExtensions
{
    private static readonly LogSource Log = new LogSource();

    /// <summary>
    /// Add a random
    /// </summary>
    /// <param name="owinConfiguration"></param>
    /// <param name="schema">string with schema to use, default is http</param>
    /// <param name="hostname">string with hostname to use, default is localhost</param>
    /// <param name="possiblePorts">optional ports to use, when nothing than a random port is used, 0 is also changed to random</param>
    /// <returns>string with the added uri</returns>
    public static IOwinConfiguration AddListenerUri(this IOwinConfiguration owinConfiguration, string schema = "http", string hostname = "localhost", params int[] possiblePorts)
    {
        owinConfiguration.ListeningUrls.Add(GenerateListenerUri(schema, hostname, possiblePorts));
        return owinConfiguration;
    }

    /// <summary>
    /// Generate a uri, with a random (not occupied) port
    /// </summary>
    /// <param name="schema">string with schema to use, default is http</param>
    /// <param name="hostname">string with hostname to use, default is localhost</param>
    /// <param name="possiblePorts">optional ports to use, when nothing than a random port is used, 0 is also changed to random</param>
    /// <returns>string with the added uri</returns>
    public static string GenerateListenerUri(string schema = "http", string hostname = "localhost", params int[] possiblePorts)
    {
        return $"{schema}://{hostname}:" + GetFreeListenerPort(possiblePorts);
    }

    /// <summary>
    ///     Returns an unused port, which savely can be used to listen to
    ///     A port of 0 in the list will have the following behaviour: https://msdn.microsoft.com/en-us/library/c6z86e63.aspx
    ///     If you do not care which local port is used, you can specify 0 for the port number. In this case, the service
    ///     provider will assign an available port number between 1024 and 5000.
    /// </summary>
    /// <param name="possiblePorts">An optional int array with ports, the routine will return the first free port.</param>
    /// <returns>A free port</returns>
    public static int GetFreeListenerPort(params int[] possiblePorts)
    {
        possiblePorts = possiblePorts == null || possiblePorts.Length == 0 ? new[] { 0 } : possiblePorts;

        var resultingPort = possiblePorts.Select(TryPort).FirstOrDefault(i => i > 0);
        if (resultingPort > 0)
        {
            return resultingPort;
        }
        var message = $"No free ports in the range {string.Join(",", possiblePorts)} found!";
        throw new NotSupportedException(message);
    }

    /// <summary>
    /// Helper method to find an unused port
    /// </summary>
    /// <param name="portToCheck">0 for random, otherwise a specific</param>
    /// <returns>The actual port, or -1 of there isn't a free port</returns>
    public static int TryPort(int portToCheck)
    {
        var listener = new TcpListener(IPAddress.Loopback, portToCheck);
        try
        {
            listener.Start();
            // As the LocalEndpoint is of type EndPoint, this doesn't have the port, we need to cast it to IPEndPoint
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            Log.Info().WriteLine("Found free listener port {0} for the WebApp.", port);
            return port;
        }
        catch
        {
            Log.Debug().WriteLine("Port {0} isn't free.", portToCheck);
        }
        finally
        {
            listener.Stop();
        }
        return -1;
    }
}