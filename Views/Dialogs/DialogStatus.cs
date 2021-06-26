using Android.Support.V7.App;
using Mettarin.Android.ViewModels;

namespace Mettarin.Android.Views.Dialogs
{
    public class DialogStatus
    {
        private readonly ProgressDialogViewModel _dialog;

        public DialogStatus(ProgressDialogViewModel dialog)
        {
            _dialog = dialog;
        }

        public void UpdateStatus(string status)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                _dialog.ActionMessage = status;
            });
        }

        public void UpdateStatus(int status)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                _dialog.ActionMessageId = status;
            });
        }
    }
}
