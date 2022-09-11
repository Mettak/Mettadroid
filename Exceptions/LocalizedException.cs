using System;

namespace Mettarin.Android.Exceptions
{
    public class LocalizedException : Exception
    {
        public object[] Args { get; }

        public int ResourceId { get; }

        public LocalizedException(int resId, Exception exception, params object[] args)
            : base(null, exception)
        {
            ResourceId = resId;
            Args = args;
        }
    }
}
