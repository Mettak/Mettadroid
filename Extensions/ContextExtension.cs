using Android.Content;
using Android.OS;
using Android.Widget;

namespace Mettarin.Android.Extensions
{
    public static class ContextExtension
    {
        public static string GetResourceString(this Context context, string resourceName)
        {
            return context.GetString(context.Resources.GetIdentifier
                (resourceName, "string", context.PackageName));
        }

        public static void ShowToast(this Context context, string text, ToastLength length = ToastLength.Long)
        {
            Handler mainHandler = new Handler(Looper.MainLooper);
            Java.Lang.Runnable runnableToast = new Java.Lang.Runnable(() =>
            {
                Toast.MakeText(context, text, length).Show();
            });

            mainHandler.Post(runnableToast);
        }

        public static void ShowToast(this Context context, int resId, ToastLength length = ToastLength.Long)
        {
            string resString = context.Resources.GetString(resId);
            ShowToast(context, resString, length);
        }
    }
}
