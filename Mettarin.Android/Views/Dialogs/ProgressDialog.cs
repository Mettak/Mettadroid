using Android.Content;
using Android.Support.V7.App;
using System;
using System.Threading.Tasks;

namespace Mettarin.Android.Views.Dialogs
{
    public class ProgressDialog : DialogBase<bool>
    {
        public Func<DialogStatus, Task> Action { get; set; }

        public int? ActionMessageId { get; set; }

        public string ActionMessage { get; set; }

        public ProgressDialog(Context context)
            : base(context) { }

        protected override void DialogInit()
        {
            if (ActionMessageId.HasValue)
            {
                _builder.SetMessage(ActionMessageId.Value);
            }

            else
            {
                _builder.SetMessage(ActionMessage);
            }
        }

        protected override AlertDialog BeforeShow()
        {
            var dialog = base.BeforeShow();

            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
            {
                var dialogStatus = new DialogStatus(dialog);
                await Action(dialogStatus);
                dialog.Cancel();
            });

            Result = true;

            return dialog;
        }
    }
}
