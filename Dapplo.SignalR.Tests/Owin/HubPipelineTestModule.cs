#region Dapplo License

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

using System;
using System.ComponentModel.Composition;
using Microsoft.AspNet.SignalR.Hubs;

namespace Dapplo.SignalR.Tests.Owin
{
    /// <summary>
    /// This helps to track exceptions, without this they are simple lost (unless tracing is turned on)
    /// </summary>
    [Export(typeof(IHubPipelineModule))]
    [Export]
    public class HubPipelineTestModule : HubPipelineModule
    {

        public Exception LatestException { get; private set; }

        /// <inheritdoc />
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            LatestException = exceptionContext.Error;
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}
