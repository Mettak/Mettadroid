using Android.Content;
using Autofac;

namespace Mettadroid
{
    public interface IModuleStartup
    {
        void ConfigureServices(Context context, ContainerBuilder builder);
    }
}
