using Android.Content;
using Android.Views;
using Android.Widget;
using Mettarin.Android.Adapters.Base;
using Mettarin.Android.Collections;
using Mettarin.Android.EventArguments;
using Mettarin.Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mettarin.Android.Adapters
{
    public abstract class AdapterBase<T> : BaseAdapter where T : ILoadableView
    {
        private bool _loading = false;

        protected readonly Context _context;

        public event EventHandler<AdapterLoadingEventArgs> OnAdapterLoaded;

        public event EventHandler<AdapterLazyLoadingEventArgs> OnLazyLoadingCompleted;

        public bool IsBusy => _loading;

        public bool Loaded { get; private set; } = false;

        public ObservableRangeCollection<T> Items { get; set; } = new ObservableRangeCollection<T>();

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

        protected abstract Task<IEnumerable<T>> GetViewModels();

        protected virtual void OnGetDataCompleted()
        {
            return;
        }

        protected virtual Task LoadDataLazily()
        {
            throw new NotImplementedException();
        }

        protected virtual Task LoadSegmentLazily(IEnumerable<T> views)
        {
            throw new NotImplementedException();
        }

        public void LazyLoading(IEnumerable<T> views)
        {
            var awaiter = LoadSegmentLazily(views).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try
                {
                    awaiter.GetResult();
                    OnLazyLoadingCompleted?.Invoke(this, new AdapterLazyLoadingEventArgs(true, null));
                }

                catch (Exception ex)
                {
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

            var getViewModelsAwaiter = GetViewModels().GetAwaiter();
            getViewModelsAwaiter.OnCompleted(() =>
            {
                try
                {
                    var viewModels = getViewModelsAwaiter.GetResult();
                    Items.Clear();
                    Items.AddRange(viewModels);
                    OnGetDataCompleted();
                    OnAdapterLoaded?.Invoke(this, new AdapterLoadingEventArgs(true, null));
                    Loaded = true;

                    if (LazyLoadingSettings == ELazyLoadingSettings.LoadEverything)
                    {
                        var loadDataLazilyAwaiter = LoadDataLazily().GetAwaiter();
                        loadDataLazilyAwaiter.OnCompleted(() =>
                        {
                            try
                            {
                                loadDataLazilyAwaiter.GetResult();
                                OnLazyLoadingCompleted?.Invoke(this, new AdapterLazyLoadingEventArgs(true, null));
                                _loading = false;
                            }

                            catch (Exception ex)
                            {
                                OnLazyLoadingCompleted?.Invoke(this, new AdapterLazyLoadingEventArgs(false, ex));
                                _loading = false;
                            }
                        });
                    }

                    else if (LazyLoadingSettings == ELazyLoadingSettings.LoadVisibleSegments)
                    {
                        var loadDataLazilyAwaiter = LoadSegmentLazily(Items.Take(50)).GetAwaiter();
                        loadDataLazilyAwaiter.OnCompleted(() =>
                        {
                            try
                            {
                                loadDataLazilyAwaiter.GetResult();
                                OnLazyLoadingCompleted?.Invoke(this, new AdapterLazyLoadingEventArgs(true, null));
                                _loading = false;
                            }

                            catch (Exception ex)
                            {
                                OnLazyLoadingCompleted?.Invoke(this, new AdapterLazyLoadingEventArgs(false, ex));
                                _loading = false;
                            }
                        });
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
