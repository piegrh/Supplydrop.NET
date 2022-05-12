using System;
using System.Collections.Generic;
using System.Linq;

namespace Webhallen.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Dictionary<TKey, TValue[]> ToLookUpDictionary<TValue, TKey>(this IEnumerable<TValue> collection, Func<TValue, TKey> keySelector)
            where TKey : notnull
            where TValue : notnull
            => collection
                .ToLookup(keySelector)
                .ToDictionary(k => k.Key, x => x.ToArray()
                );
    }
}
