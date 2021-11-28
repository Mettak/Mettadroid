namespace Mettarin.Android.Views
{
    public interface IStatusView
    {
        void UpdateStatus(string status);

        void UpdateStatus(int status);

        void UpdateProgress(int newProgress);
    }
}
