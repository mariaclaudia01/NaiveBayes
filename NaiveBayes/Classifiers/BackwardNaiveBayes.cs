using System.Collections.Generic;
using System.Linq;
using NaiveBayes.CollectionExtensions;
using NaiveBayes.IO;

namespace NaiveBayes.Classifiers
{
    public class BackwardNaiveBayes: ITrainable
    {
        private readonly BasicGlobalProbability basicGlobalProbability = new BasicGlobalProbability();

        public Dictionary<string, Dictionary<string, Dictionary<string, int>>> Statistics = 
            new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

        public void Train(List<WordPartOfSpeech> trainingSet)
        {
            basicGlobalProbability.Train(trainingSet);

            var predecessor = trainingSet.FirstOrDefault();

            foreach (var item in trainingSet.Skip(1))
            {
                Train(item, predecessor);
                predecessor = item;
            }
        }

        private void Train(WordPartOfSpeech current, WordPartOfSpeech predecessor)
        {
            Statistics
                .GetOrCreate(current.Word)
                .GetOrCreate(predecessor.PartOfSpeech)
                .GetOrCreate(current.PartOfSpeech);

            Statistics[current.Word][predecessor.PartOfSpeech][current.PartOfSpeech]++;
        }

        public Dictionary<string, double> Probabilities(string word, string predecessorPartOfSpeech)
        {
            return Statistics.GetOrCreate(word).GetOrCreate(predecessorPartOfSpeech).Statistics();
        }

        public string PartOfSpeech(string word, string predecessorPartOfSpeech)
        {
            if (predecessorPartOfSpeech == PartsOfSpeech.EndOfSentence)
            {
                return basicGlobalProbability.PartOfSpeech(word);
            }

            var partOfSpeechProbabilities = Probabilities(word, predecessorPartOfSpeech);

            if (partOfSpeechProbabilities.Any())
            {
                return partOfSpeechProbabilities.KeyWithMaxValue();
            }
            return basicGlobalProbability.PartOfSpeech(word);
        }

        public double Accuracy(List<WordPartOfSpeech> testSet)
        {
            var predecessorPartOfSpeech = "";
            int successful = 0;

            foreach (var item in testSet)
            {
                var predictedPartOfSpeech = PartOfSpeech(item.Word, predecessorPartOfSpeech);
               
                if (predictedPartOfSpeech == item.PartOfSpeech)
                {
                    successful++;
                }

                predecessorPartOfSpeech = predictedPartOfSpeech;
            }
           
            return (double)successful / testSet.Count();
        }

        public double Precision(string partOfSpeech, List<WordPartOfSpeech> testSet)
        {
            var predecessorPartOfSpeech = "";
            int truePositive = 0;
            int falsePositive = 0;

            foreach (var item in testSet)
            {
                var predictedPartOfSpeech = PartOfSpeech(item.Word, predecessorPartOfSpeech);

                if (predictedPartOfSpeech == item.PartOfSpeech && predictedPartOfSpeech == partOfSpeech)
                {
                    truePositive++;
                }

                if (predictedPartOfSpeech != item.PartOfSpeech && predictedPartOfSpeech == partOfSpeech)
                {
                    falsePositive++;
                }

                predecessorPartOfSpeech = predictedPartOfSpeech;
            }

            return (double)truePositive / (truePositive + falsePositive);
        }

        public double Recall(string partOfSpeech, List<WordPartOfSpeech> testSet)
        {
            var predecessorPartOfSpeech = "";
            int truePositive = 0;
            int falseNegative = 0;

            foreach (var item in testSet)
            {
                var predictedPartOfSpeech = PartOfSpeech(item.Word, predecessorPartOfSpeech);

                if (predictedPartOfSpeech == item.PartOfSpeech && predictedPartOfSpeech == partOfSpeech)
                {
                    truePositive++;
                }

                if (predictedPartOfSpeech != item.PartOfSpeech && predictedPartOfSpeech != partOfSpeech)
                {
                    falseNegative++;
                }

                predecessorPartOfSpeech = predictedPartOfSpeech;
            }

            return (double)truePositive / (truePositive + falseNegative);
        }
    }
}
