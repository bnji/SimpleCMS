using SimpleCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class ControllerExtensions
    {
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            List<T> list = destination as List<T>;
            if (list != null)
            {
                list.AddRange(source);
            }
            else
            {
                foreach (T item in source)
                {
                    destination.Add(item);
                }
            }
        }

        public static IEnumerable<TObject> FilterPublished<TObject>(this IEnumerable<TObject> list) where TObject : class, IPublishableContent
        {
            return list.Where(x => FilterOutUnPublished(x));
        }

        public static IEnumerable<TObject> FilterDeleted<TObject>(this IEnumerable<TObject> list) where TObject : class
        {
            return list.Where(x => FilterOutDeleted(x));
        }

        public static IEnumerable<TObject> Find<TObject>(this IEnumerable<TObject> list, Func<TObject, bool> predicate) where TObject : class
        {
            return list.Where(x => FilterOutDeleted(x)).Where(predicate);
        }

        //public static IQueryable<TObject> FilterDeleted<TObject>(this IQueryable<TObject> list) where TObject : class
        //{
        //    return list.Where(x => FilterOutDeleted(x));
        //}

        public static bool FilterOutDeleted<TObject>(TObject input) where TObject : class
        {
            var changeEvent = input as IHasChangeEvent;
            if (changeEvent != null)
                return changeEvent.ChangeEventDeletedOn == null;
            return true;
        }

        public static bool FilterOutUnPublished<TObject>(TObject x) where TObject : class, IPublishableContent
        {
            return x.IsPublished && (!x.ActiveFrom.HasValue || DateTime.Compare(DateTime.UtcNow, x.ActiveFrom.Value) >= 0) && (!x.ActiveTo.HasValue || DateTime.Compare(DateTime.UtcNow, x.ActiveTo.Value) <= 0);
        }

    }
}