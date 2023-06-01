using System.Collections.Generic;
using System.Linq;

namespace Anshan.Domain
{
    public static class ValueObjectExtensions
    {
        public static void Update<T>(this IList<T> currentList,
                                     IList<T> list)
        {
            var added = list.Except(currentList).ToList();
            var removed = currentList.Except(list).ToList();

            added.ForEach(currentList.Add);
            removed.ForEach(a => currentList.Remove(a));
        }

        public static void Update<T>(this IList<T> currentList, T oldObject, T newObject)
        {
            currentList.Remove(oldObject);
            currentList.Add(newObject);
        }
    }
}