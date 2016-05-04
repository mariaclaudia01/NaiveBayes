using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json;

namespace NaiveBayes
{
    class Program
    {
        private static BasicGlobalProbability basicGlobalProbability;

        private static void Main(string[] args)
        {
            var reader = new TrainingSetReader();
            var trainingSet = reader.Read("training.xml");
            basicGlobalProbability = new BasicGlobalProbability(trainingSet);
            SaveStatistics("output.json");

            Console.WriteLine("Press CTRL+C to stop the program");
            
            while (true)
            {
                string word = Console.ReadLine();
                PrintMaxProbabilityPartOfSpeech(word);
            }
        }      

        private static void SaveStatistics(string filename)
        {
            string output = JsonConvert.SerializeObject(basicGlobalProbability.Statistics, Formatting.Indented);
            File.WriteAllText(filename, output);
        }

        private static void PrintMaxProbabilityPartOfSpeech(string word)
        {
            var partOfSpeechProbabilities = basicGlobalProbability.Probabilities(word);

            if (partOfSpeechProbabilities.Any())
            {
                var maxProbabilityPartOfSpeech = partOfSpeechProbabilities.OrderBy(p => p.Probability).Last();
                Console.WriteLine(maxProbabilityPartOfSpeech.PartOfSpeech);
            }
            else
            {
                Console.WriteLine("Word not found");
            }
        }
    }
}
