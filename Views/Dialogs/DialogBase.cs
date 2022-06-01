using Android.Content;
using AndroidX.AppCompat.App;
using System;
using System.Threading.Tasks;

namespace Mettarin.Android.Views.Dialogs
{
    public abstract class DialogBase<ReturnType>
    {
        protected readonly Context _context;

        protected readonly AlertDialog.Builder _builder;

        private TaskCompletionSource<ReturnType> _tcs = new TaskCompletionSource<ReturnType>();

        private ReturnType _result = default;

        protected ReturnType Result
        {
            get => _result;
            set
            {
                _result = value;
                _tcs.SetResult(_result);
            }
        }

        public string Title { get; set; }

        public int? TitleTextId { get; set; }

        public int? ThemeResourceId { get; set; }

        protected virtual bool Cancelable { get; } = false;

        private AlertDialog _dialog;

        protected AlertDialog Dialog => _dialog;

        public DialogBase(Context context)
        {
            _context = context;

            if (ThemeResourceId.HasValue)
            {
                _builder = new AlertDialog.Builder(_context, ThemeResourceId.Value);
            }

            else
            {
                var styleId = context.Resources.GetIdentifier("Mettarin.Dialog.Alert", "style", context.PackageName);
                if (styleId != 0)
                {
                    _builder = new AlertDialog.Builder(_context, styleId);
                }

                else
                {
                    _builder = new AlertDialog.Builder(_context);
                }
            }
        }

        protected abstract void DialogInit();

        protected virtual AlertDialog BeforeShow()
        {
            AlertDialog dialog = null;

            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                dialog = _builder.Show();
                dialog.CancelEvent += (s, e) =>
                {
                    if (Cancelable && (Result?.Equals(default) ?? false))
                    {
                        Result = default;
                    }
                };
            });

            return dialog;
        }

        public virtual void Show(Action<ReturnType> continuation = null)
        {
            var awaiter = ShowAndWaitForResult().GetAwaiter();

            if (continuation != null)
            {
                Action newAction = new Action(() =>
                {
                    continuation(awaiter.GetResult());
                });

                awaiter.OnCompleted(newAction);
            }
        }

        public virtual async Task<ReturnType> ShowAndWaitForResult()
        {
            _result = default;
            _tcs = new TaskCompletionSource<ReturnType>();

            _builder.SetCancelable(Cancelable);

            if (TitleTextId.HasValue)
            {
                _builder.SetTitle(TitleTextId.Value);
            }

            else
            {
                _builder.SetTitle(Title);
            }
            
            DialogInit();
            _dialog = BeforeShow();

            return await _tcs.Task;
        }
    }
}
