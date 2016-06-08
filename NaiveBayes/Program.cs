using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using NaiveBayes.Classifiers;
using NaiveBayes.CollectionExtensions;
using NaiveBayes.IO;

namespace NaiveBayes
{
    class Program
    {
        private static List<WordPartOfSpeech> trainingSet;
        private static List<WordPartOfSpeech> testSet;

        private static void Main(string[] args)
        {
            Read("data.xml");

            Evaluate(new BasicGlobalProbability());
            Evaluate(new BackwardNaiveBayes());

            Console.ReadKey();
        }

        private static void Read(string filename)
        {
            var reader = new LabeledDataReader();
            var data = reader.Read(filename);

            trainingSet = data.TakePercent(0.7);
            testSet = data.SkipPercent(0.7);
        }

        private static void Evaluate(ITrainable classifier)
        {
            classifier.Train(trainingSet);
            Save(classifier);

            Console.WriteLine(classifier);
            ComputeAccuracy(classifier);
            ComputePrecision(classifier);
            ComputeRecall(classifier);
        }

        private static void Save(ITrainable classifier)
        {
            string statistics = JsonConvert.SerializeObject(classifier, Formatting.Indented);
            File.WriteAllText(classifier + ".json", statistics);
        }

        private static void ComputeAccuracy(ITrainable classifier)
        {
            double accuracy = classifier.Accuracy(testSet);
            Console.WriteLine("Accuracy: " + accuracy);
        }

        private static void ComputePrecision(ITrainable classifier)
        {
            Console.WriteLine("Precision:");
            foreach (var partOfSpeech in PartsOfSpeech.All())
            {
                double precision = classifier.Precision(partOfSpeech, testSet);
                Console.WriteLine(partOfSpeech + ": " + precision);
            }
        }

        private static void ComputeRecall(ITrainable classifier)
        {
            Console.WriteLine("Recall:");
            foreach (var partOfSpeech in PartsOfSpeech.All())
            {
                double recall = classifier.Recall(partOfSpeech, testSet);
                Console.WriteLine(partOfSpeech + ": " + recall);
            }
        }
    }
}
