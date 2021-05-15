using Android.Content;
using Autofac;

namespace Mettarin.Android
{
    public interface IModuleStartup
    {
        void ConfigureServices(Context context, ContainerBuilder builder);
    }
}
