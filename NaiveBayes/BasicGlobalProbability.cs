using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BasicGlobalProbability: IPredictor
    {
        public readonly StatisticsDictionary PartOfSpeechStatistics = new StatisticsDictionary();

        public readonly Dictionary<string, StatisticsDictionary> WordStatistics =
            new Dictionary<string, StatisticsDictionary>();           
        
        public void Train(IEnumerable<WordPartOfSpeech> trainingSet)
        {
            foreach (var trainingSetItem in trainingSet)
            {
                CountOccurrence(trainingSetItem);
            }
        }

        private void CountOccurrence(WordPartOfSpeech item)
        {
            if (!WordStatistics.ContainsKey(item.Word))
                WordStatistics[item.Word] = new StatisticsDictionary();

            WordStatistics[item.Word].Increment(item.PartOfSpeech);
            PartOfSpeechStatistics.Increment(item.PartOfSpeech);
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
