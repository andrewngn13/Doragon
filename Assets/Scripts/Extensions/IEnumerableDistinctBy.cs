using System;
using System.Collections.Generic;
using System.Linq;

namespace Doragon.Extensions
{
    public static class IEnumerableDistinctBy
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
                else
                {

                }
            }
        }
    }
}