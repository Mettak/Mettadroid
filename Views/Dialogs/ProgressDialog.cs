using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Threading.Tasks;

namespace Mettarin.Android.Views.Dialogs
{
    public class ProgressDialogViewModel : IView
    {
        private readonly TextView _textView;

        private int? _actionMessageId;

        public int? ActionMessageId
        {
            get => _actionMessageId;
            set
            {
                _actionMessageId = value;
                if (value.HasValue)
                {
                    _textView.SetText(value.Value);
                }
            }
        }

        public string ActionMessage
        {
            get => _textView.Text;
            set => _textView.Text = value;
        }

        public Color ActionMessageColor
        {
            get => new Color(_textView.CurrentTextColor);
            set => _textView.SetTextColor(value);
        }

        public View View { get; }

        public ProgressDialogViewModel(Context context)
        {
            LayoutInflater inflater;

            var styleId = context.Resources.GetIdentifier("Mettarin.Dialog.Alert", "style", context.PackageName);
            if (styleId != 0)
            {
                inflater = LayoutInflater.From(context).CloneInContext(new ContextThemeWrapper(context, styleId));
            }

            else
            {
                inflater = LayoutInflater.From(context);
            }

            View = inflater.Inflate(Resource.Layout.mettarin_progress_dialog, null, false);
            _textView = View.FindViewById<TextView>(Resource.Id.text_view);
        }
    }

    public class ProgressDialog : CustomDialogBase<bool, ProgressDialogViewModel>
    {
        public Func<DialogStatus, Task<bool>> Action { get; set; }

        public ProgressDialog(Context context, ProgressDialogViewModel viewModel) : base(context, viewModel)
        { }

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
