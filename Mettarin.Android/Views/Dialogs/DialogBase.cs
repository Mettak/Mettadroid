using Android.Content;
using Android.Support.V7.App;
using System;
using System.Threading.Tasks;

namespace Mettarin.Android.Views.Dialogs
{
    public abstract class DialogBase<T>
    {
        protected readonly Context _context;

        protected readonly AlertDialog.Builder _builder;

        protected readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();

        private T _result = default;

        protected T Result
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

        public DialogBase(Context context)
        {
            _context = context;
            _builder = new AlertDialog.Builder(_context);
        }

        protected abstract void DialogInit();

        protected virtual AlertDialog BeforeShow()
        {
            AlertDialog dialog = null;

            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                 dialog = _builder.Show();
            });

            return dialog;
        }

        public virtual void Show(Action<T> continuation = null)
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

        public virtual async Task<T> ShowAndWaitForResult()
        {
            _result = default;

            _builder.SetCancelable(false);

            if (TitleTextId.HasValue)
            {
                _builder.SetTitle(TitleTextId.Value);
            }

            else
            {
                _builder.SetTitle(Title);
            }

            DialogInit();
            BeforeShow();

            return await _tcs.Task;
        }
    }
}
