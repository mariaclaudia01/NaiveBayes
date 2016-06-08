using System.Collections.Generic;
using System.Linq;
using NaiveBayes.CollectionExtensions;
using NaiveBayes.IO;

namespace NaiveBayes.Classifiers
{
    public class ForwardNaiveBayes : ITrainable
    {
        private readonly BasicGlobalProbability basicGlobalProbability = new BasicGlobalProbability();

        public Dictionary<string, Dictionary<string, Dictionary<string, int>>> Statistics =
            new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

        public void Train(List<WordPartOfSpeech> trainingSet)
        {
            basicGlobalProbability.Train(trainingSet);

            var reversedTrainingSet = trainingSet.Backwards();
            var successor = reversedTrainingSet.FirstOrDefault();

            foreach (var item in reversedTrainingSet.Skip(1))
            {
                Train(item, successor);
                successor = item;
            }
        }

        private void Train(WordPartOfSpeech current, WordPartOfSpeech successor)
        {
            Statistics
                .GetOrCreate(current.Word)
                .GetOrCreate(successor.PartOfSpeech)
                .GetOrCreate(current.PartOfSpeech);

            Statistics[current.Word][successor.PartOfSpeech][current.PartOfSpeech]++;
        }

        public Dictionary<string, double> Probabilities(string word, string successorPartOfSpeech)
        {
            return Statistics.GetOrCreate(word).GetOrCreate(successorPartOfSpeech).Statistics();
        }

        public string PartOfSpeech(string word, string successorPartOfSpeech)
        {
            if (successorPartOfSpeech == PartsOfSpeech.EndOfSentence)
            {
                return basicGlobalProbability.PartOfSpeech(word);
            }

            var partOfSpeechProbabilities = Probabilities(word, successorPartOfSpeech);

            if (partOfSpeechProbabilities.Any())
            {
                return partOfSpeechProbabilities.KeyWithMaxValue();
            }
            return basicGlobalProbability.PartOfSpeech(word);
        }

        public double Accuracy(List<WordPartOfSpeech> testSet)
        {
            var successorPartOfSpeech = "";
            int successful = 0;

            foreach (var item in testSet.Backwards())
            {
                var predictedPartOfSpeech = PartOfSpeech(item.Word, successorPartOfSpeech);

                if (predictedPartOfSpeech == item.PartOfSpeech)
                {
                    successful++;
                }

                successorPartOfSpeech = predictedPartOfSpeech;
            }

            return (double)successful / testSet.Count();
        }

        public double Precision(string partOfSpeech, List<WordPartOfSpeech> testSet)
        {
            var successorPartOfSpeech = "";
            int truePositive = 0;
            int falsePositive = 0;

            foreach (var item in testSet.Backwards())
            {
                var predictedPartOfSpeech = PartOfSpeech(item.Word, successorPartOfSpeech);

                if (predictedPartOfSpeech == item.PartOfSpeech && predictedPartOfSpeech == partOfSpeech)
                {
                    truePositive++;
                }

                if (predictedPartOfSpeech != item.PartOfSpeech && predictedPartOfSpeech == partOfSpeech)
                {
                    falsePositive++;
                }

                successorPartOfSpeech = predictedPartOfSpeech;
            }

            return (double)truePositive / (truePositive + falsePositive);
        }

        public double Recall(string partOfSpeech, List<WordPartOfSpeech> testSet)
        {
            var successorPartOfSpeech = "";
            int truePositive = 0;
            int falseNegative = 0;

            foreach (var item in testSet.Backwards())
            {
                var predictedPartOfSpeech = PartOfSpeech(item.Word, successorPartOfSpeech);

                if (predictedPartOfSpeech == item.PartOfSpeech && predictedPartOfSpeech == partOfSpeech)
                {
                    truePositive++;
                }

                if (predictedPartOfSpeech != item.PartOfSpeech && predictedPartOfSpeech != partOfSpeech)
                {
                    falseNegative++;
                }

                successorPartOfSpeech = predictedPartOfSpeech;
            }

            return (double)truePositive / (truePositive + falseNegative);
        }
    }
}
