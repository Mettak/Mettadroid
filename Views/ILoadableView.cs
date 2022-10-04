namespace Mettadroid.Views
{
    public interface ILoadableView : IView
    {
        bool IsLoaded { get; }

        bool IsLoading { get; set; }
    }
}
