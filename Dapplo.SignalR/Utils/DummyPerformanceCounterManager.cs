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

using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
		private static readonly PropertyInfo[] CounterProperties = GetCounterPropertyInfo();
		private static readonly IPerformanceCounter NoOpCounter = new NoOpPerformanceCounter();

		/// <summary>
		/// This initializes a dummy performance counter, which can be used as a workaround for some issues.
		/// </summary>
		public DummyPerformanceCounterManager()
		{
			foreach (var property in CounterProperties)
			{
				property.SetValue(this, new NoOpPerformanceCounter(), null);
			}
		}

		/// <inheritdoc />
		public void Initialize(string instanceName, CancellationToken hostShutdownToken)
		{
		}

		/// <inheritdoc />
		public IPerformanceCounter LoadCounter(string categoryName, string counterName, string instanceName, bool isReadOnly)
		{
			return NoOpCounter;
		}

		internal static PropertyInfo[] GetCounterPropertyInfo()
		{
			return typeof(DummyPerformanceCounterManager)
				.GetProperties()
				.Where(p => p.PropertyType == typeof(IPerformanceCounter))
				.ToArray();
		}

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsConnected { get; set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsReconnected { get; set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsDisconnected { get; set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsCurrentForeverFrame { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsCurrentLongPolling { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsCurrentServerSentEvents { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsCurrentWebSockets { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionsCurrent { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionMessagesReceivedTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionMessagesSentTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionMessagesReceivedPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ConnectionMessagesSentPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusMessagesReceivedTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusMessagesReceivedPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ScaleoutMessageBusMessagesReceivedPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusMessagesPublishedTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusMessagesPublishedPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusSubscribersCurrent { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusSubscribersTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusSubscribersPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusAllocatedWorkers { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusBusyWorkers { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter MessageBusTopicsCurrent { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsAllTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsAllPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsHubResolutionTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsHubResolutionPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsHubInvocationTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsHubInvocationPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsTransportTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ErrorsTransportPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ScaleoutStreamCountTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ScaleoutStreamCountOpen { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ScaleoutStreamCountBuffering { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ScaleoutErrorsTotal { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ScaleoutErrorsPerSec { get; private set; }

		/// <inheritdoc />
		public IPerformanceCounter ScaleoutSendQueueLength { get; private set; }
	}

	internal class NoOpPerformanceCounter : IPerformanceCounter
	{
		public string CounterName => GetType().Name;

		public long Decrement()
		{
			return 0;
		}

		public long Increment()
		{
			return 0;
		}

		public long IncrementBy(long value)
		{
			return 0;
		}

		public long RawValue
		{
			get { return 0; }
			set { }
		}

		public void Close()
		{
		}

		public void RemoveInstance()
		{
		}

		public CounterSample NextSample()
		{
			return CounterSample.Empty;
		}
	}
}
