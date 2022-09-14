using Android.Content;
using Android.Content.PM;
using Autofac;
using Java.Lang;
using Mettarin.Android;
using Mettarin.Android.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mettarin
{
    public static class DependencyInjection
    {
        private static IContainer _container;

        public static T GetService<T>(params object[] parameters)
        {
            if (_container == null)
            {
                throw new System.Exception("Dependency injection not initialized!");
            }

            var autofacParams = new List<Autofac.Core.Parameter>();
            foreach (var parameter in parameters)
            {
                autofacParams.Add(new TypedParameter(parameter.GetType(), parameter));
            }
            
            return _container.Resolve<T>(autofacParams);
        }

        public static T GetService<T>(Context context, params object[] parameters)
        {
            if (_container == null)
            {
                throw new System.Exception("Dependency injection not initialized!");
            }

            var autofacParams = new List<Autofac.Core.Parameter>();
            autofacParams.Add(TypedParameter.From(context));
            foreach (var parameter in parameters)
            {
                autofacParams.Add(new TypedParameter(parameter.GetType(), parameter));
            }

            return _container.Resolve<T>(autofacParams);
        }

        public static object GetService(Type serviceType, params object[] parameters)
        {
            if (_container == null)
            {
                throw new System.Exception("Dependency injection not initialized!");
            }

            var autofacParams = new List<Autofac.Core.Parameter>();
            foreach (var parameter in parameters)
            {
                autofacParams.Add(new TypedParameter(parameter.GetType(), parameter));
            }

            return _container.Resolve(serviceType, autofacParams);
        }

        public static object GetService(Type serviceType, Context context, params object[] parameters)
        {
            if (_container == null)
            {
                throw new System.Exception("Dependency injection not initialized!");
            }

            var autofacParams = new List<Autofac.Core.Parameter>();
            autofacParams.Add(TypedParameter.From(context));
            foreach (var parameter in parameters)
            {
                autofacParams.Add(new TypedParameter(parameter.GetType(), parameter));
            }

            return _container.Resolve(serviceType, autofacParams);
        }

        public static void ConfigureServices(Context context)
        {
            if (_container != null)
            {
                return;
            }

            ContainerBuilder builder = new ContainerBuilder();

            var mettaringConfig = StartupBase.GetConfiguration<Configuration>(
                context, sectionName: "MettarinConfiguration") ?? new Configuration();
            mettaringConfig.ModulePrefixes.Insert(0, nameof(Mettarin));
            builder.RegisterInstance<Configuration>(mettaringConfig).SingleInstance();

            GetModules(mettaringConfig.ModulePrefixes).ForEach(x =>
            {
                x.ConfigureServices(context, builder);
            });

            _container = builder.Build();
        }

        private static List<IModuleStartup> GetModules(List<string> prefixes)
        {
            var startupTypes = new List<IModuleStartup>();
            var types = AssemblyHelpers.GetTypesFromAssembliesWithPrefixex(prefixes.ToArray()).Where(
                x => typeof(IModuleStartup).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

            types.ForEach(objectType =>
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
