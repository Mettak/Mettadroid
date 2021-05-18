using System;

namespace Mettarin.Android.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SortableAttribute : Attribute
    {
        public string DefaultSortPropertyName { get; }

        public SortableAttribute(string defaultSortPropertyName)
        {
            DefaultSortPropertyName = defaultSortPropertyName;
        }
    }
}
