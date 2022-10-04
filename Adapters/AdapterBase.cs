using Android.Content;
using Android.Views;
using Android.Widget;
using Mettadroid.Adapters.Base;
using Mettadroid.Collections;
using Mettadroid.EventArguments;
using Mettadroid.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mettadroid.Adapters
{
    public abstract class AdapterBase<T> : BaseAdapter where T : ILoadableView
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private bool _loading = false;

        protected readonly Context _context;

        public event EventHandler<AdapterLoadingEventArgs> OnAdapterLoaded;

        public event EventHandler<AdapterLazyLoadingEventArgs> OnLazyLoadingCompleted;

        public bool IsBusy => _loading;

        public bool Loaded { get; private set; } = false;

        public AdapterObservableCollection<T> Items { get; } = new AdapterObservableCollection<T>();

        public virtual ELazyLoadingSettings LazyLoadingSettings { get; } = ELazyLoadingSettings.Disabled;

        public AdapterBase(Context context)
        {
            _context = context;
            Items.CollectionChanged += ViewModel_CollectionChanged;
        }

        private void ViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(NotifyDataSetChanged);
        }

        protected abstract IEnumerable<T> GetViewModels();

        internal Task<IEnumerable<T>> GetViewModelsInternal()
        {
            return Task.Run(() =>
            {
                return GetViewModels();
            });
        }

        protected virtual void OnGetViewModelsCompleted() { return; }

        internal Task LoadSegmentsLazilyInternal(IEnumerable<T> views)
        {
            return Task.Run(() =>
            {
                _semaphore.Wait();

                try
                {
                    LoadSegmentLazily(views);
                }

                finally
                {
                    _semaphore.Release();
                }
            });
        }

        protected virtual void LoadSegmentLazily(IEnumerable<T> viewsInSegment)
        {
            throw new NotImplementedException();
        }

        public void LazyLoading(IEnumerable<T> views)
        {
            _loading = true;
            var awaiter = LoadSegmentsLazilyInternal(views).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try
                {
                    awaiter.GetResult();
                    _loading = false;
                    OnLazyLoadingCompleted?.Invoke(this, new AdapterLazyLoadingEventArgs(true, null));
                }

                catch (Exception ex)
                {
                    _loading = false;
                    OnLazyLoadingCompleted?.Invoke(this, new AdapterLazyLoadingEventArgs(false, ex));
                }
            });
        }

        public void Load()
        {
            if (_loading)
            {
                return;
            }

            _loading = true;

            var getViewModelsAwaiter = GetViewModelsInternal().GetAwaiter();
            getViewModelsAwaiter.OnCompleted(() =>
            {
                try
                {
                    var viewModels = getViewModelsAwaiter.GetResult();
                    Items.Clear();
                    Items.AddRange(viewModels);
                    OnGetViewModelsCompleted();
                    OnAdapterLoaded?.Invoke(this, new AdapterLoadingEventArgs(true, null));
                    Loaded = true;

                    if (LazyLoadingSettings == ELazyLoadingSettings.LoadEverything)
                    {
                        LazyLoading(Items);
                    }

                    else if (LazyLoadingSettings == ELazyLoadingSettings.LoadVisibleSegments)
                    {
                        LazyLoading(Items.Take(50));
                    }

                    else
                    {
                        _loading = false;
                    }
                }

                catch (Exception ex)
                {
                    OnAdapterLoaded?.Invoke(this, new AdapterLoadingEventArgs(false, ex));
                }
            });
        }

        public override Java.Lang.Object GetItem(int position) => Items[position].View;

        public override int GetItemViewType(int position) => position;

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent) => Items[position].View;

        public override int Count => Items.Count;
    }
}
