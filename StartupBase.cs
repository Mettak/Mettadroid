using Android.Content;
using Autofac;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Mettarin.Android
{
    public abstract class StartupBase : IModuleStartup
    {
        public abstract void ConfigureServices(Context context, ContainerBuilder builder);

        public T GetConfiguration<T>(Context context, string configFile = "appconfig.json", string sectionName = "")
        {
            using var input = context.Assets.Open(configFile);
            using var streamReader = new StreamReader(input);
            var appConfiguration = JObject.Parse(streamReader.ReadToEnd());

            if (!string.IsNullOrEmpty(sectionName))
            {
                appConfiguration = appConfiguration.Value<JObject>(sectionName);
            }

            return appConfiguration.ToObject<T>();
        }
    }
}
