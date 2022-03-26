namespace Mettarin.Android.Views.Dialogs
{
    public class DialogStatus : IStatusView
    {
        private readonly ProgressDialogViewModel _dialog;

        public DialogStatus(ProgressDialogViewModel dialog)
        {
            _dialog = dialog;
        }

        public void UpdateProgress(int newProgress)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                _dialog.ActionMessage = $"{newProgress}%";
            });
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
