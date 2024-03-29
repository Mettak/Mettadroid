﻿using Android.Graphics;
using Android.Views;
using Xamarin.Essentials;

namespace Mettadroid.Extensions
{
    public static class ViewExtension
    {
        public static bool IsPointInView(this View view, int x, int y)
        {
            if (view == null)
            {
                return false;
            }

            if (!view.IsShown)
            {
                return false;
            }

            Rect actualPosition = new Rect();
            view.GetGlobalVisibleRect(actualPosition);
            return actualPosition.Contains(x, y);
        }

        public static bool IsVisible(this View view)
        {
            if (view == null)
            {
                return false;
            }

            if (!view.IsShown)
            {
                return false;
            }

            Rect actualPosition = new Rect();
            view.GetGlobalVisibleRect(actualPosition);
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            Rect screen = new Rect(0, 0, (int)displayInfo.Width, (int)displayInfo.Height);
            return actualPosition.Intersect(screen);
        }
    }
}
