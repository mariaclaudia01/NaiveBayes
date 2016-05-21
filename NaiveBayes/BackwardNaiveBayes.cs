using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BackwardNaiveBayes
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, int>>> Statistics = 
            new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

        public void Train(List<WordPartOfSpeech> trainingSet)
        {
            var predecessor = trainingSet.FirstOrDefault();

            foreach (var trainingItem in trainingSet.Skip(1))
            {
                Train(trainingItem, predecessor);
                predecessor = trainingItem;
            }
        }

        private void Train(WordPartOfSpeech current, WordPartOfSpeech predecessor)
        {
            Statistics
                .GetOrCreate(current.Word)
                .GetOrCreate(current.PartOfSpeech)
                .GetOrCreate(predecessor.PartOfSpeech);
            
            Statistics[current.Word][current.PartOfSpeech][predecessor.PartOfSpeech]++;
        }

        public Dictionary<string, double> Probabilities(string word, string previousPartOfSpeech)
        {
            return new Dictionary<string, double>();
        }

        public string PredictPartOfSpeech(string word)
        {
            return "";
        }
    }
}
