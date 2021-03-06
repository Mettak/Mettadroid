using Android.Content;

namespace Mettarin.Android.Views.Dialogs
{
    public class QuestionDialog : DialogBase<bool>
    {
        public string Question { get; set; }

        public int? QuestionTextId { get; set; }

        public int YesButtonTextId { get; set; } = Resource.String.mettarin_yes;

        public int NoButtonTextId { get; set; } = Resource.String.mettarin_no;

        public QuestionDialog(Context context)
            : base(context)
        { }

        protected override void DialogInit()
        {
            if (QuestionTextId.HasValue)
            {
                _builder.SetMessage(QuestionTextId.Value);
            }

            else
            {
                _builder.SetMessage(Question);
            }

            _builder.SetPositiveButton(YesButtonTextId, handler: (s, e) =>
            {
                Result = true;
            });
            _builder.SetNegativeButton(NoButtonTextId, handler: (s, e) =>
            {
                Result = false;
            });
        }
    }
}
