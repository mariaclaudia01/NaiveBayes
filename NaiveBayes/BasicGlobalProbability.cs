using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BasicGlobalProbability
    {
        public readonly Dictionary<string, Dictionary<string, int>> Statistics =
            new Dictionary<string, Dictionary<string, int>>();

        public BasicGlobalProbability(IEnumerable<WordPartOfSpeech> trainingSet)
        {
            foreach (var trainingSetItem in trainingSet)
            {
                CountOccurrence(trainingSetItem);
            }
        }

        private void CountOccurrence(WordPartOfSpeech item)
        {
            if (!Statistics.ContainsKey(item.Word))
                Statistics[item.Word] = new Dictionary<string, int>();

            if (!Statistics[item.Word].ContainsKey(item.PartOfSpeech))
                Statistics[item.Word][item.PartOfSpeech] = 0;

            Statistics[item.Word][item.PartOfSpeech]++;
        }

        public List<PartOfSpeechProbability> Probabilities(string word)
        {
            if (!Statistics.ContainsKey(word))
                return new List<PartOfSpeechProbability>();

            return PartsOfSpeech(word).Select(
                partOfSpeech => new PartOfSpeechProbability
                {
                    PartOfSpeech = partOfSpeech,
                    Probability = Probability(word, partOfSpeech)
                }).ToList();
        }

        private IEnumerable<string> PartsOfSpeech(string word)
        {
            return Statistics[word].Keys;
        }

        private double Probability(string word, string partOfSpeech)
        {
            return (double)Statistics[word][partOfSpeech] / TotalOccurrences(word);
        }

        private long TotalOccurrences(string word)
        {
            return Statistics[word].Keys
                .Sum(partOfSpeech => Statistics[word][partOfSpeech]);
        }
    }
}
