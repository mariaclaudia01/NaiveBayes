using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public static class ListPercentageExtensions
    {
        public static List<T> SkipPercent<T>(this List<T> sequence, double percentage)
        {
            return sequence.Skip(sequence.Count(percentage)).ToList();
        }

        public static List<T> TakePercent<T>(this List<T> sequence, double percentage)
        {
            return sequence.Take(sequence.Count(percentage)).ToList();
        }

        private static int Count<T>(this List<T> sequence, double percentage)
        {
            return (int)(sequence.Count() * percentage);
        }
    }
}
