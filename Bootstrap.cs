using Android.Content;
using Android.Content.PM;
using Autofac;
using Java.Lang;
using Mettarin.Android;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mettarin
{
    public static class Bootstrap
    {
        private static IContainer _container;

        public static T GetService<T>(params object[] parameters)
        {
            var autofacParams = new List<Autofac.Core.Parameter>();
            foreach (var parameter in parameters)
            {
                autofacParams.Add(new TypedParameter(parameter.GetType(), parameter));
            }

            return _container.Resolve<T>(autofacParams);
        }

        public static T GetService<T>(Context context, params object[] parameters)
        {
            var autofacParams = new List<Autofac.Core.Parameter>();
            autofacParams.Add(TypedParameter.From(context));
            foreach (var parameter in parameters)
            {
                autofacParams.Add(new TypedParameter(parameter.GetType(), parameter));
            }

            return _container.Resolve<T>(autofacParams);
        }

        public static void ConfigureServices(Context context)
        {
            if (_container != null)
            {
                return;
            }

            ContainerBuilder builder = new ContainerBuilder();

            GetModules().ForEach(x =>
            {
                x.ConfigureServices(context, builder);
            });

            _container = builder.Build();
        }

        private static List<IModuleStartup> GetModules()
        {
            var objectTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x =>
                {
                    try
                    {
                        return typeof(IModuleStartup).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract;
                    }

                    catch
                    {
                        return false;
                    }
                }).Select(x => x).ToList();
            var startupTypes = new List<IModuleStartup>();

            objectTypes.ForEach(objectType =>
            {
                var instance = (IModuleStartup)Activator.CreateInstance(objectType);
                startupTypes.Add(instance);
            });

            return startupTypes;
        }

        public static void RestartApp(Context context)
        {
            PackageManager packageManager = context.PackageManager;
            Intent intent = packageManager.GetLaunchIntentForPackage(context.PackageName);
            ComponentName componentName = intent.Component;
            Intent mainIntent = Intent.MakeRestartActivityTask(componentName);
            context.StartActivity(mainIntent);
            Runtime.GetRuntime().Exit(0);
        }
    }
}
