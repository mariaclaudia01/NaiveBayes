using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NaiveBayes
{
    class Program
    {
        private static BasicGlobalProbability basicGlobalProbability;

        private static void Main(string[] args)
        {
            var trainingSet = ReadTrainingSet("training.xml");
            basicGlobalProbability = new BasicGlobalProbability(trainingSet);

            Console.WriteLine("Press CTRL+C to stop the program");
            
            while (true)
            {
                string word = Console.ReadLine();
                PrintMaxProbabilityPartOfSpeech(word);
            }
        }

        private static IEnumerable<WordPartOfSpeech> ReadTrainingSet(string filename)
        {
            return from word in XDocument.Load("training.xml")
                .XPathSelectElements("/text/sentence/word")
                select new WordPartOfSpeech
                {
                    Word = word.Attribute("form").Value,
                    PartOfSpeech = word.Attribute("postag").Value
                };
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
