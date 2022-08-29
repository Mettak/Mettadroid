using System;
using System.Collections.Generic;
using System.Linq;

namespace Mettarin.Android.Helpers
{
    public static class AssemblyHelpers
    {
        public static List<Type> GetTypesFromAssembliesWithPrefixex(params string[] prefixes)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => prefixes.Any(y => x.FullName.StartsWith(y))).ToList();
            return loadedAssemblies.SelectMany(x => x.GetTypes()).ToList();
        }
    }
}
