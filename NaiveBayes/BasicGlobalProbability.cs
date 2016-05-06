using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BasicGlobalProbability
    {
        public readonly Dictionary<string, int> PartOfSpeechStatistics =
           new Dictionary<string, int>(); 

        public readonly Dictionary<string, Dictionary<string, int>> WordStatistics =
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
            if (!WordStatistics.ContainsKey(item.Word))
                WordStatistics[item.Word] = new Dictionary<string, int>();

            Increment(WordStatistics[item.Word], item.PartOfSpeech);
            Increment(PartOfSpeechStatistics, item.PartOfSpeech);
        }

        private void Increment(Dictionary<string, int> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = 0;
            }
            dictionary[key]++;   
        }

        public List<PartOfSpeechProbability> Probabilities(string word)
        {
            if (!WordStatistics.ContainsKey(word))
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
            return WordStatistics[word].Keys;
        }

        private double Probability(string word, string partOfSpeech)
        {
            return (double)WordStatistics[word][partOfSpeech] / TotalOccurrences(word);
        }

        private long TotalOccurrences(string word)
        {
            return PartsOfSpeech(word)
                .Sum(partOfSpeech => WordStatistics[word][partOfSpeech]);
        }      
    }
}
