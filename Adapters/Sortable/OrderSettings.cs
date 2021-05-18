using Mettarin.Android.Attributes;
using System;
using System.Reflection;

namespace Mettarin.Android.Adapters.Sortable
{
    public class OrderSettings<TSource>
    {
        public ESortOrder SortOrder { get; }

        public PropertyInfo PropertyInfo { get; }

        public OrderSettings(ESortOrder sortOrder, string propertyName)
        {
            SortOrder = sortOrder;
            var type = typeof(TSource);
            var attribute = type.GetCustomAttribute<SortableAttribute>();

            if (attribute == null)
            {
                throw new Exception($"This type does not have {nameof(SortableAttribute)}");
            }

            if (propertyName == null)
            {
                PropertyInfo defaultProperty = type.GetProperty(attribute.DefaultSortPropertyName);
                PropertyInfo = defaultProperty ?? throw new Exception($"Property {attribute.DefaultSortPropertyName} not found");
            }

            else
            {
                PropertyInfo prop = type.GetProperty(propertyName);
                PropertyInfo = prop ?? throw new Exception($"Property {propertyName} not found");
            }
        }
    }
}
