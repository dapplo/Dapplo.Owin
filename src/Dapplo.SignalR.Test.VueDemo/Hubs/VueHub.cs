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
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Dapplo.Log;
using Dapplo.SignalR.Test.VueDemo.Model;
using Microsoft.AspNet.SignalR;

namespace Dapplo.SignalR.Test.VueDemo.Hubs;

/// <summary>
///     The Vue demo Hub
/// </summary>
public class VueHub : Hub<IVueHubClient>, IVueHubServer
{
    private static readonly LogSource Log = new LogSource();
    private readonly MyVueModel _myVueModel;

    /// <summary>
    /// Constructor taking the global singleton IMyVueModel
    /// </summary>
    /// <param name="myVueModel">IMyVueModel</param>
    public VueHub(MyVueModel myVueModel)
    {
        _myVueModel = myVueModel;
        _myVueModel.PropertyChanged += MyVueModelOnPropertyChanged;
    }

    /// <inheritdoc />
    public override async Task OnConnected()
    {
        await base.OnConnected().ConfigureAwait(false);
            
        Log.Debug().WriteLine("Connect from {0}", Context.User?.Identity?.Name);
        await Clients.Caller.UpdateModel(_myVueModel);
    }

    /// <summary>
    /// The server calls all clients when something changes
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">PropertyChangedEventArgs</param>
    private async void MyVueModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Skip propagation when the change came from the web page
        if ("Hub".Equals(_myVueModel.Source))
        {
            await Clients.All.UpdateModel(_myVueModel);
            return;
        }
        _myVueModel.Source = "Hub";
    }

    /// <summary>
    /// Called from (one of) the client(s)
    /// </summary>
    /// <param name="myVueModel">MyVueModel</param>
    public async Task StoreModelChange(MyVueModel myVueModel)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _myVueModel.Source = myVueModel.Source;
            _myVueModel.Name = myVueModel.Name;
        });

        // Update all other clients
        await Clients.Others.UpdateModel(_myVueModel);
    }
}