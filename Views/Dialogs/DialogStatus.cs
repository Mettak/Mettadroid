using Android.Support.V7.App;

namespace Mettarin.Android.Views.Dialogs
{
    public class DialogStatus
    {
        private readonly AlertDialog _dialog;

        public DialogStatus(AlertDialog dialog)
        {
            _dialog = dialog;
        }

        public void UpdateStatus(string status)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                _dialog.SetMessage(status);
            });
        }

        public void UpdateStatus(int status)
        {
            string statusLocalized = _dialog.Context.Resources.GetString(status);
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                _dialog.SetMessage(statusLocalized);
            });
        }
    }
}
