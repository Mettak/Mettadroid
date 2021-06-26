using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Mettarin.Android.Adapters;
using Mettarin.Android.Adapters.Base;
using Mettarin.Android.Extensions;
using System;
using System.Linq;

namespace Mettarin.Android.Views.GridViews
{
    public abstract class GridViewBase<AdapterClass, ViewModel> : GridView where AdapterClass : AdapterBase<ViewModel> where ViewModel : ILoadableView
    {
        public new AdapterClass Adapter
        {
            get => base.Adapter as AdapterClass;
            set
            {
                base.Adapter = value;
                if (!value.Loaded)
                {
                    value.Load();
                }
            }
        }

        public GridViewBase(Context context) : base(context)
        { }

        public GridViewBase(Context context, IAttributeSet attrs) : base(context, attrs)
        { }

        private float _previousY = 0;

        protected virtual void LazyLoading(MotionEvent e)
        {
            if (Adapter.LazyLoadingSettings == ELazyLoadingSettings.LoadVisibleSegments)
            {
                switch (e.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        _previousY = e.RawY;
                        break;

                    case MotionEventActions.Up:
                        var diff = Math.Abs(e.RawY - _previousY);
                        if (diff > 0)
                        {
                            var visibleViews = Adapter.Items.Where(x => !x.IsLoaded && !x.IsLoading);
                            if (!visibleViews.Any())
                            {
                                break;
                            }

                            visibleViews = visibleViews.Where(x => x.View.IsVisible());
                            Adapter.LazyLoading(visibleViews);
                        }
                        break;
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            LazyLoading(e);
            return base.OnTouchEvent(e);
        }
    }
}
