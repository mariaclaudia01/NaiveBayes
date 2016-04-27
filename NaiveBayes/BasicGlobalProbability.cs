using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BasicGlobalProbability
    {
        private readonly Dictionary<string, Dictionary<string, int>> occurrences =
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
            if (!occurrences.ContainsKey(item.Word))
                occurrences[item.Word] = new Dictionary<string, int>();

            if (!occurrences[item.Word].ContainsKey(item.PartOfSpeech))
                occurrences[item.Word][item.PartOfSpeech] = 0;

            occurrences[item.Word][item.PartOfSpeech]++;
        }

        public List<PartOfSpeechProbability> Probabilities(string word)
        {
            if (!occurrences.ContainsKey(word)) 
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
            return occurrences[word].Keys;
        }

        private double Probability(string word, string partOfSpeech)
        {
            return (double)occurrences[word][partOfSpeech] / TotalOccurrences(word);
        }

        private long TotalOccurrences(string word)
        {
            return occurrences[word].Keys
                .Sum(partOfSpeech => occurrences[word][partOfSpeech]);
        }

        public Dictionary<string, Dictionary<string, double>> AllProbabilities()
        {
            return occurrences.ToDictionary(
                x => x.Key, 
                x => x.Value.ToDictionary(
                    y=>y.Key, 
                    y=>Probability(x.Key, y.Key)));
        }
    }
}
