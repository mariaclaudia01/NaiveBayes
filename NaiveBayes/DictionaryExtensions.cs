using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
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

        public static TKey KeyWithMaxValue<TKey>(this Dictionary<TKey, int> dictionary)
        {
            return dictionary.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
