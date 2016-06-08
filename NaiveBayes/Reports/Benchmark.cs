using System;
using System.Collections.Generic;
using NaiveBayes.Classifiers;
using NaiveBayes.IO;

namespace NaiveBayes.Reports
{
    public class Benchmark
    {
        private readonly ITrainable classifier;
        private readonly List<WordPartOfSpeech> testSet;

        public Benchmark(ITrainable classifier, List<WordPartOfSpeech> testSet)
        {
            this.classifier = classifier;
            this.testSet = testSet;
        }

        public string Report()
        {
            var report = string.Empty;
            report += ClassifierName();
            report += ClassifierAccuracy();
            report += ClassifierPrecision();
            report += ClassifierRecall();
            return report;
        }

        private string ClassifierName()
        {
            return classifier + Environment.NewLine;
        }

        private string ClassifierAccuracy()
        {
            double accuracy = classifier.Accuracy(testSet);
            return "Accuracy: " + accuracy + Environment.NewLine;
        }

        private string ClassifierPrecision()
        {
            var report = "Precision:" + Environment.NewLine;

            foreach (var partOfSpeech in PartsOfSpeech.All())
            {
                double precision = classifier.Precision(partOfSpeech, testSet);
                report += partOfSpeech + ": " + precision + Environment.NewLine;
            }

            return report;
        }

        private string ClassifierRecall()
        {
            var report = "Recall:" + Environment.NewLine;

            foreach (var partOfSpeech in PartsOfSpeech.All())
            {
                double recall = classifier.Recall(partOfSpeech, testSet);
                report += partOfSpeech + ": " + recall + Environment.NewLine;
            }

            return report;
        }
    }
}
