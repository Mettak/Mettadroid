﻿using Android.Content;
using Mettadroid.Adapters.Sortable;
using Mettadroid.Views;
using System.Linq;

namespace Mettadroid.Adapters
{
    public abstract class AdapterSortable<T> : AdapterBase<T> where T : ILoadableView
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

        protected override void OnGetViewModelsCompleted()
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
