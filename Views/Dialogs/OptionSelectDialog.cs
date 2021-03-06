using Android.Content;
using System.Collections.Generic;
using System.Linq;

namespace Mettarin.Android.Views.Dialogs
{
    public class OptionSelectDialog : DialogBase<int>
    {
        public List<int> Options { get; set; } = new List<int>();

        public List<string> OptionsStrings { get; set; } = new List<string>();

        public OptionSelectDialog(Context context) : base(context)
        { }

        protected override void DialogInit()
        {
            if (Options.Any())
            {
                var optionsLocalized = new Dictionary<int, string>();
                foreach (var option in Options)
                {
                    optionsLocalized.Add(option, _context.Resources.GetString(option));
                }

                _builder.SetItems(optionsLocalized.Select(x => x.Value).ToArray(), handler: (s, e) =>
                {
                    Result = Options[e.Which];
                });
            }

            else
            {
                _builder.SetItems(OptionsStrings.ToArray(), handler: (s, e) =>
                {
                    Result = e.Which;
                });
            }
        }
    }
}
