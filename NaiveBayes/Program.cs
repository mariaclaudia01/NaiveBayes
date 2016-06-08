using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using NaiveBayes.Classifiers;
using NaiveBayes.CollectionExtensions;
using NaiveBayes.IO;
using NaiveBayes.Reports;

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
            Evaluate(new ForwardNaiveBayes());

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

            var benchmark = new Benchmark(classifier, testSet);
            Console.WriteLine(benchmark.Report());
        }

        private static void Save(ITrainable classifier)
        {
            string statistics = JsonConvert.SerializeObject(classifier, Formatting.Indented);
            File.WriteAllText(classifier + ".json", statistics);
        }
    }
}
