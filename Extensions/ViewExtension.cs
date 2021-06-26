using Android.Graphics;
using Android.Views;
using Xamarin.Essentials;

namespace Mettarin.Android.Extensions
{
    public static class ViewExtension
    {
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
