using System;
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

            var serializer = new JsonSerializer(classifier);
            serializer.SerializeTo(classifier + ".json");

            var benchmark = new Benchmark(classifier, testSet);
            Console.WriteLine(benchmark.Report());
        }
    }
}
