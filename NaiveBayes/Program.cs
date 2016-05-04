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
            var wordXmlTags = ReadWordXmlTags(filename);
            int trainingItemsCount = (int)(wordXmlTags.Count() * 0.7);
            return wordXmlTags
                .Select(Parse)
                .Where(wordPartOfSpeech => IsValid(wordPartOfSpeech.Word))
                .Take(trainingItemsCount);
        }
        
        private static IEnumerable<XElement> ReadWordXmlTags(string filename)
        {
            return XDocument.Load(filename)
               .XPathSelectElements("/text/sentence/word");
        }

        private static WordPartOfSpeech Parse(XElement wordXmlTag) 
        {
            return new WordPartOfSpeech
                {
                    Word = wordXmlTag.Attribute("lemma").Value.ToLower(),
                    PartOfSpeech = wordXmlTag.Attribute("postag").Value[0].ToString().ToLower()
                };
        }
        private static bool IsValid(string word)
        {
            return Regex.IsMatch(word, @"^[a-z_]+$");
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
