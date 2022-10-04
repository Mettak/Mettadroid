using System;

namespace Mettadroid.Exceptions
{
    public class LocalizedException : Exception
    {
        public object[] Args { get; }

        public int ResourceId { get; }

        public LocalizedException(int resourceId, params object[] args)
            : base(null, null)
        {
            ResourceId = resourceId;
            Args = args;
        }

        public LocalizedException(int resId, Exception exception, params object[] args)
            : base(null, exception)
        {
            ResourceId = resId;
            Args = args;
        }
    }
}
