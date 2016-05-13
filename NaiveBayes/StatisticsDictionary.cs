using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class StatisticsDictionary: Dictionary<string, int>
    {
        public void Increment(string key)
        {
            if (!ContainsKey(key))
            {
                this[key] = 0;
            }
            this[key]++;
        }

        public string KeyWithMaxValue()
        {
            return this.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
