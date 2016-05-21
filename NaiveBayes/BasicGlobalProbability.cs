using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BasicGlobalProbability: ITrainable
    {
        public readonly Dictionary<string, int> PartOfSpeechStatistics = new Dictionary<string, int>();

        public readonly Dictionary<string, Dictionary<string, int>> WordStatistics =
            new Dictionary<string, Dictionary<string, int>>();           
        
        public void Train(List<WordPartOfSpeech> trainingSet)
        {
            trainingSet.ForEach(Train);
        }

        private void Train(WordPartOfSpeech item)
        {
            WordStatistics.GetOrCreate(item.Word).GetOrCreate(item.PartOfSpeech);
            WordStatistics[item.Word][item.PartOfSpeech]++;

            PartOfSpeechStatistics.GetOrCreate(item.PartOfSpeech);
            PartOfSpeechStatistics[item.PartOfSpeech]++;
        }

        public string PartOfSpeech(string word)
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

        public double Accuracy(List<WordPartOfSpeech> testSet)
        {
            int successfulPredictions = testSet
                .Count(item => PartOfSpeech(item.Word) == item.PartOfSpeech);

            return (double)successfulPredictions / testSet.Count;
        }
    }
}
