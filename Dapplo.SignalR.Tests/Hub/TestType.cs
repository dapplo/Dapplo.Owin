using System;
using System.Reactive.Subjects;

namespace Dapplo.SignalR.Tests.Hub
{
    /// <summary>
    /// A class for testing
    /// </summary>
    public class TestType
    {
        public string Message { get; set; }

        /// <summary>
        /// This should not cause issues, due to the CamelCaseContractResolver
        /// </summary>
        public IObservable<string> AnObservable { get; } = new Subject<string>();
    }
}
