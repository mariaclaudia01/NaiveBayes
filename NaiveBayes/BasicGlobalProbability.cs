using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BasicGlobalProbability
    {
        public readonly Dictionary<string, int> PartOfSpeechStatistics = new Dictionary<string, int>();

        public readonly Dictionary<string, Dictionary<string, int>> WordStatistics =
            new Dictionary<string, Dictionary<string, int>>();           
        
        public void Train(IEnumerable<WordPartOfSpeech> trainingSet)
        {
            foreach (var trainingSetItem in trainingSet)
            {
                CountOccurrence(trainingSetItem);
            }
        }

        private void CountOccurrence(WordPartOfSpeech item)
        {
            WordStatistics.GetOrCreate(item.Word).GetOrCreate(item.PartOfSpeech);
            WordStatistics[item.Word][item.PartOfSpeech]++;

            PartOfSpeechStatistics.GetOrCreate(item.PartOfSpeech);
            PartOfSpeechStatistics[item.PartOfSpeech]++;
        }

        public string PredictPartOfSpeech(string word)
        {
            var partOfSpeechProbabilities = Probabilities(word);

            if (partOfSpeechProbabilities.Any())
            {
                return partOfSpeechProbabilities.OrderBy(p => p.Probability).Last().PartOfSpeech;
            }
            return PartOfSpeechStatistics.KeyWithMaxValue();
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
