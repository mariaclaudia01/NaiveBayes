using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var trainingSet = ReadTrainingSet("training.xml");
            basicGlobalProbability = new BasicGlobalProbability(trainingSet);
            SaveProbabilities("output.json");

            Console.WriteLine("Press CTRL+C to stop the program");
            
            while (true)
            {
                string word = Console.ReadLine();
                PrintMaxProbabilityPartOfSpeech(word);
            }
        }

        private static IEnumerable<WordPartOfSpeech> ReadTrainingSet(string filename)
        {
            return from word in XDocument.Load(filename)
                .XPathSelectElements("/text/sentence/word")
                select new WordPartOfSpeech
                {
                    Word = word.Attribute("lemma").Value.ToLower(),
                    PartOfSpeech = word.Attribute("postag").Value[0].ToString().ToLower()
                };
        }

        private static void SaveProbabilities(string filename)
        {
            var probabilities = basicGlobalProbability.AllProbabilities();
            string output = JsonConvert.SerializeObject(probabilities, Formatting.Indented);
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
