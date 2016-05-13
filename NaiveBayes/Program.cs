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

            var trainingSet = data.TakePercent(0.7);
            basicGlobalProbability = new BasicGlobalProbability(trainingSet);
            SaveStatistics("output.json");

            var testSet = data.SkipPercent(0.7);
            ComputeAccuracy(testSet);
            Console.ReadKey();
        }

        private static void SaveStatistics(string filename)
        {
            string statistics = JsonConvert.SerializeObject(basicGlobalProbability, Formatting.Indented);
            File.WriteAllText(filename, statistics);
        }

        private static void ComputeAccuracy(List<WordPartOfSpeech> testSet)
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
