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

using System.ComponentModel;
using System.Threading.Tasks;
using Dapplo.SignalR.Test.VueDemo.Model;
using Dapplo.SignalR.Test.VueDemo.Model.Impl;
using Microsoft.AspNet.SignalR;

namespace Dapplo.SignalR.Test.VueDemo.Hubs
{
    /// <summary>
    ///     The share context hub
    /// </summary>
    public class VueHub : Hub<IVueHubClient>, IVueHubServer
    {
        private readonly IMyVueModel _myVueModel;

        /// <summary>
        /// Constructor taking the global singleton IMyVueModel
        /// </summary>
        /// <param name="myVueModel">IMyVueModel</param>
        public VueHub(IMyVueModel myVueModel)
        {
            _myVueModel = myVueModel;
            _myVueModel.PropertyChanged += MyVueModelOnPropertyChanged;
        }

        /// <inheritdoc />
        public override async Task OnConnected()
        {
            await base.OnConnected().ConfigureAwait(false);
            await Clients.Caller.UpdateModel(_myVueModel);
        }

        /// <summary>
        /// The server calls all clients when something changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void MyVueModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Clients.All.UpdateModel(_myVueModel);
        }

        /// <summary>
        /// Called from the client
        /// </summary>
        /// <param name="myVueModel">IMyVueModel</param>
        public Task StoreModelChange(MyVueModel myVueModel)
        {
            _myVueModel.Name = myVueModel.Name;
            return Task.CompletedTask;
        }
    }
}
