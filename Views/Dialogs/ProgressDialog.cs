using Android.Content;
using Android.Support.V7.App;
using Mettarin.Android.ViewModels;
using System;
using System.Threading.Tasks;

namespace Mettarin.Android.Views.Dialogs
{
    public class ProgressDialog : DialogBase<bool>
    {
        public Func<DialogStatus, Task<bool>> Action { get; set; }

        public ProgressDialogViewModel ViewModel { get; set; }

        public ProgressDialog(Context context)
            : base(context) { }

        protected override void DialogInit()
        {
            if (ViewModel == null)
            {
                throw new NullReferenceException(nameof(ViewModel));
            }

            _builder.SetView(ViewModel.View);
        }

        protected override AlertDialog BeforeShow()
        {
            var dialog = base.BeforeShow();

            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
            {
                var dialogStatus = new DialogStatus(ViewModel);
                var result = await Action(dialogStatus);
                dialog.Cancel();
                Result = result;
            });

            return dialog;
        }
    }
}
