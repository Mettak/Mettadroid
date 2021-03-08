using Android.Content;
using Android.Views;
using Android.Widget;
using Mettarin.Android.Collections;
using Mettarin.Android.Views;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mettarin.Android.Adapters
{
    public abstract class AdapterBase<T> : BaseAdapter where T : IView
    {
        private bool _loading = false;

        protected readonly Context _context;

        public event EventHandler OnAdapterLoaded;

        public event EventHandler OnLazyLoadingCompleted;

        public ObservableRangeCollection<T> Items { get; set; } = new ObservableRangeCollection<T>();

        protected virtual bool LazyLoading { get; } = false;

        public AdapterBase(Context context)
        {
            _context = context;
            Items.CollectionChanged += ViewModel_CollectionChanged;
        }

        private void ViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(NotifyDataSetChanged);
        }

        protected abstract Task GetDataAsync();

        protected virtual Task LoadDataLazily()
        {
            throw new NotImplementedException();
        }

        public Task Load()
        {
            return Task.Run(async () =>
            {
                if (_loading)
                {
                    return;
                }

                _loading = true;

                await GetDataAsync();
                OnAdapterLoaded?.Invoke(this, EventArgs.Empty);

                if (LazyLoading)
                {
                    await LoadDataLazily();
                    OnLazyLoadingCompleted?.Invoke(this, EventArgs.Empty);
                }

                _loading = false;
            });
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return Items[position].View;
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return Items.ElementAt(position).View;
        }

        public override int Count
        {
            get
            {
                return Items.Count;
            }
        }
    }
}
