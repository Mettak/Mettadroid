using System;

namespace Mettarin.Android.EventArguments
{
    public class AdapterLazyLoadingEventArgs : EventArgs
    {
        public bool Succeeded { get; }

        public Exception Exception { get; }

        public AdapterLazyLoadingEventArgs(bool succeeded, Exception exception)
        {
            Succeeded = succeeded;
            Exception = exception;
        }
    }
}
