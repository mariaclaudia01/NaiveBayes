using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using NaiveBayes.Classifiers;

namespace NaiveBayes.IO
{
    class LabeledDataReader
    {
        public List<WordPartOfSpeech> Read(string filename)
        {
            return XDocument.Load(filename)
                .XPathSelectElements("/text/sentence/word")
                .Select(Parse)
                .Where(IsValid)
                .ToList();
        }
        
        private WordPartOfSpeech Parse(XElement wordXmlTag)
        {
            return new WordPartOfSpeech
            {
                Word = wordXmlTag.Attribute("lemma").Value.ToLower(),
                PartOfSpeech = Normalize(wordXmlTag.Attribute("postag").Value.ToLower())
            };
        }

        readonly Dictionary<char, string> map = new Dictionary<char, string>
        {
            	 {'n', "subst"},
                 {'m', "subst"},
                 {'d', "subst"},
                 {'v', "verb"},
                 {'a', "adj"},
                 {'r', "adv"},
                 {'t', "adv"},
                 {'q', "articol"},
                 {'c', "articol"},
            	 {'s', "prepozitie"},
                 {'y', "prepozitie"},
                 {'p', "articol"}, 
        };

        readonly List<string> punctuation = new List<string>
        {
           "punct","punct.","PERIOD", "."
        };

        string Normalize(string posTag)
        {
            if (punctuation.Contains(posTag))
                return "punct";

            return map.ContainsKey(posTag[0])
                ? map[posTag[0]]
                : "altele";
        }

        private bool IsValid(WordPartOfSpeech trainingItem)
        {
            if (trainingItem.Word == ".") return true;
            if (trainingItem.Word == "_") return false;
            return Regex.IsMatch(trainingItem.Word, @"^[a-z_]*$");
        }
    }
}
