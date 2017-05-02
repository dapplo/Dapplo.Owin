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
using Microsoft.AspNet.SignalR.Infrastructure;

namespace Dapplo.SignalR.Utils
{
    /// <summary>
    /// Implementation for a No-Operation performance counter
    /// </summary>
    internal class NoOpPerformanceCounter : IPerformanceCounter
    {
        public string CounterName => GetType().Name;

        public long Decrement()
        {
            return --RawValue;
        }

        public long Increment()
        {
            return ++RawValue;
        }

        public long IncrementBy(long value)
        {
            RawValue += value;
            return RawValue;
        }

        public long RawValue { get; set; }

        public void Close()
        {
            // Do nothing
        }

        public void RemoveInstance()
        {
            // Do nothing
        }

        public CounterSample NextSample()
        {
            return CounterSample.Empty;
        }
    }
}