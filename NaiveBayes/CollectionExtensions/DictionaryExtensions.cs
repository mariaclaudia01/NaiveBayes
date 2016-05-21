using System;
using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes.CollectionExtensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue val;

            if (!dictionary.TryGetValue(key, out val))
            {
                val = new TValue();
                dictionary.Add(key, val);
            }

            return val;
        }

        public static TKey KeyWithMaxValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TValue: IComparable
        {
            return dictionary.Aggregate((l, r) => l.Value.CompareTo(r.Value) == 1 ? l : r).Key;
        }

        public static Dictionary<TKey, double> Statistics<TKey>(this Dictionary<TKey, int> dictionary)
        {
            return dictionary.ToDictionary(
                item => item.Key,
                item => (double) dictionary[item.Key]/dictionary.Values.Sum());
        }
    }
}
