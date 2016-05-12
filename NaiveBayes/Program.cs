using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NaiveBayes
{
    class Program
    {
        private static BasicGlobalProbability basicGlobalProbability;

        private static void Main(string[] args)
        {
            var reader = new LabeledDataReader();
            var data = reader.Read("data.xml");

            var trainingSet = Extract(data, 0, 0.7);
            basicGlobalProbability = new BasicGlobalProbability(trainingSet);
            SaveStatistics("output.json");

            var testSet = Extract(data, 0.7, 1);
            ComputeAccuracy(testSet);
        }

        private static IEnumerable<T> Extract<T>(IEnumerable<T> elements, double startPercent, double stopPercent)
        {
            int x = (int)(elements.Count() * startPercent);
            int y = (int)(elements.Count() * (stopPercent - startPercent));

            return elements.Skip(x).Take(y);
        }

        private static void SaveStatistics(string filename)
        {
            string statistics = JsonConvert.SerializeObject(basicGlobalProbability, Formatting.Indented);
            File.WriteAllText(filename, statistics);
        }

        private static void ComputeAccuracy(IEnumerable<WordPartOfSpeech> testSet)
        {
            int successfulPredictions = testSet
                .Where(IsSuccessfulPrediction)
                .Count();

            Console.WriteLine((double)successfulPredictions / testSet.Count());
        }

        private static bool IsSuccessfulPrediction(WordPartOfSpeech testItem)
        {
            return basicGlobalProbability.PredictPartOfSpeech(testItem.Word) == testItem.PartOfSpeech;
        }
    }
}
