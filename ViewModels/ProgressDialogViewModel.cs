using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Mettarin.Android.Views;

namespace Mettarin.Android.ViewModels
{
    public class ProgressDialogViewModel : IView
    {
        private readonly Context _context;

        private readonly ProgressBar _progressBar;

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
            _context = context;

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
            _progressBar = View.FindViewById<ProgressBar>(Resource.Id.progress_bar);
            _textView = View.FindViewById<TextView>(Resource.Id.text_view);
        }
    }
}
