using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mettarin.Android.Collections
{
    public class AdapterObservableCollection<T> : ObservableCollection<T>
    {
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Items.Add(item);
            }

            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(
                System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

        public void ForEach(Action<T> action)
        {
            foreach (var item in Items)
            {
                action(item);
            }
        }
    }
}
