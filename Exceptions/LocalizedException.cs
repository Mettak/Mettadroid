using System;

namespace Mettarin.Android.Exceptions
{
    public class LocalizedException : Exception
    {
        public int ResourceId { get; }

        public LocalizedException(int resId, Exception exception)
            : base(null, exception)
        {
            ResourceId = resId;
        }
    }
}
