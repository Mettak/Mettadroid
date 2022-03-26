using Android.Content;

namespace Mettarin.Android.Views.Dialogs
{
    public abstract class CustomDialogBase<ReturnType, ViewModelType> : DialogBase<ReturnType> where ViewModelType : IView
    {
        private readonly ViewModelType _viewModel;

        public ViewModelType ViewModel => _viewModel;

        protected CustomDialogBase(Context context, ViewModelType viewModel) : base(context)
        {
            _viewModel = viewModel;
        }

        protected override void DialogInit()
        {
            _builder.SetView(_viewModel.View);
        }
    }
}
