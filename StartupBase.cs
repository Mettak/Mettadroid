using Android.Content;
using Autofac;

namespace Mettarin.Android
{
    public abstract class StartupBase : IModuleStartup
    {
        public abstract void ConfigureServices(Context context, ContainerBuilder builder);
    }
}
