using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NaiveBayes
{
    class Program
    {
        private static IPredictor predictor;
        private static List<WordPartOfSpeech> trainingSet;
        private static List<WordPartOfSpeech> testSet;

        private static void Main(string[] args)
        {
            Read("data.xml");
            Train();                 
            SaveStatistics("output.json");
            ComputeAccuracy();
            Console.ReadKey();
        }

        private static void Read(string filename)
        {
            var reader = new LabeledDataReader();
            var data = reader.Read(filename);

            trainingSet = data.TakePercent(0.7);
            testSet = data.SkipPercent(0.7);
        }

        private static void Train()
        {
            predictor = new BasicGlobalProbability();
            predictor.Train(trainingSet);
        }

        private static void SaveStatistics(string filename)
        {
            string statistics = JsonConvert.SerializeObject(predictor, Formatting.Indented);
            File.WriteAllText(filename, statistics);
        }

        private static void ComputeAccuracy()
        {
            var accuracy = new Accuracy(predictor);
            Console.WriteLine(accuracy.Compute(testSet));
        }        
    }
}
