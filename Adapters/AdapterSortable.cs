using Android.Content;
using Mettarin.Android.Adapters.Sortable;
using Mettarin.Android.Views;
using System.Linq;

namespace Mettarin.Android.Adapters
{
    public abstract class AdapterSortable<T> : AdapterBase<T> where T : IView
    {
        protected abstract ESortOrder DefaultSortOrder { get; }

        protected abstract string DefaultSortOrderPropertyName { get; }

        protected OrderSettings<T> OrderSettings { get; set; }

        public AdapterSortable(Context context, OrderSettings<T> orderSettings = null) : base(context)
        {
            if (orderSettings == null)
            {
                OrderSettings = new OrderSettings<T>(DefaultSortOrder, DefaultSortOrderPropertyName);
            }

            else
            {
                OrderSettings = orderSettings;
            }
        }

        protected override void OnGetDataCompleted()
        {
            Sort();
        }

        public void Sort()
        {
            Sort(OrderSettings);
        }

        public void Sort(OrderSettings<T> orderSettings)
        {
            if (orderSettings != OrderSettings)
            {
                OrderSettings = orderSettings;
            }

            if (OrderSettings.SortOrder == ESortOrder.Ascending)
            {
                var sorted = Items.OrderBy(x => OrderSettings.PropertyInfo.GetValue(x, null)).ToList();
                Items.Clear();
                Items.AddRange(sorted);
            }

            else
            {
                var sorted = Items.OrderByDescending(x => OrderSettings.PropertyInfo.GetValue(x, null)).ToList();
                Items.Clear();
                Items.AddRange(sorted);
            }
        }
    }
}
