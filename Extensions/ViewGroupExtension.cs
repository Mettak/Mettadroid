using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mettadroid.Extensions
{
    public static class ViewGroupExtension
    {
        public static List<T> FindViewsByType<T>(this ViewGroup viewGroup, params int[] exceptions) where T : View
        {
            List<T> views = new List<T>();

            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                var view = viewGroup.GetChildAt(i);

                if (view is T targetView)
                {
                    if (exceptions.Contains(view.Id))
                    {
                        continue;
                    }

                    else
                    {
                        views.Add(targetView);
                    }
                }

                else if (view is ViewGroup group)
                {
                    views.AddRange(FindViewsByType<T>(group));
                }

                else
                {
                    continue;
                }
            }

            return views;
        }
    }
}
