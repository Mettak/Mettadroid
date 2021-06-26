using System;

namespace Mettarin.Android.EventArguments
{
    public class AdapterLoadingEventArgs : EventArgs
    {
        public bool Succeeded { get; }

        public Exception Exception { get; }

        public AdapterLoadingEventArgs(bool succeeded, Exception exception)
        {
            Succeeded = succeeded;
            Exception = exception;
        }
    }
}