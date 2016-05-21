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
            foreach (var item in trainingSet)
            {
                Train(item);
            }
        }

        private void Train(WordPartOfSpeech item)
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
                return partOfSpeechProbabilities.KeyWithMaxValue();
            }
            return PartOfSpeechStatistics.KeyWithMaxValue();
        }

        public Dictionary<string, double> Probabilities(string word)
        {
            return WordStatistics.GetOrCreate(word).Statistics();
        }
    }
}
