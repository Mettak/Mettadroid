﻿using Android.Content;

namespace Mettadroid.Views.Dialogs
{
    public class SimpleMessageDialog : DialogBase<bool>
    {
        public string Message { get; set; }

        public int? MessageTextId { get; set; }

        public int ButtonTextId { get; set; } = Resource.String.mettarin_ok;

        public SimpleMessageDialog(Context context)
            : base(context)
        { }

        protected override void DialogInit()
        {
            if (MessageTextId.HasValue)
            {
                _builder.SetMessage(MessageTextId.Value);
            }

            else
            {
                _builder.SetMessage(Message);
            }

            _builder.SetPositiveButton(ButtonTextId, handler: (s, e) =>
            {
                Result = true;
            });
        }
    }
}
