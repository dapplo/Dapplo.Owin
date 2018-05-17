#region Dapplo License

//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2018 Dapplo
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

using System.Threading;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace Dapplo.SignalR.Utils
{
    /// <summary>
    /// Build after the TempPerformanceCounterManager <a href="https://github.com/SignalR/SignalR/issues/3414">here</a>
    /// This is a workaround which solves some issues in performance and reduces some exceptions.
    /// </summary>
    public class DummyPerformanceCounterManager : IPerformanceCounterManager
    {
        private static readonly IPerformanceCounter NoOpCounter = new NoOpPerformanceCounter();

        /// <inheritdoc />
        public void Initialize(string instanceName, CancellationToken hostShutdownToken)
        {
            // Do nothing
        }

        /// <inheritdoc />
        public IPerformanceCounter LoadCounter(string categoryName, string counterName, string instanceName, bool isReadOnly)
        {
            return NoOpCounter;
        }

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsConnected { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsReconnected { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsDisconnected { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsCurrentForeverFrame { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsCurrentLongPolling { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsCurrentServerSentEvents { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsCurrentWebSockets { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionsCurrent { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionMessagesReceivedTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionMessagesSentTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionMessagesReceivedPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ConnectionMessagesSentPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusMessagesReceivedTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusMessagesReceivedPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ScaleoutMessageBusMessagesReceivedPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusMessagesPublishedTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusMessagesPublishedPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusSubscribersCurrent { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusSubscribersTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusSubscribersPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusAllocatedWorkers { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusBusyWorkers { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter MessageBusTopicsCurrent { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsAllTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsAllPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsHubResolutionTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsHubResolutionPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsHubInvocationTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsHubInvocationPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsTransportTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ErrorsTransportPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ScaleoutStreamCountTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ScaleoutStreamCountOpen { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ScaleoutStreamCountBuffering { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ScaleoutErrorsTotal { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ScaleoutErrorsPerSec { get; } = NoOpCounter;

        /// <inheritdoc />
        public IPerformanceCounter ScaleoutSendQueueLength { get; } = NoOpCounter;
    }
}
