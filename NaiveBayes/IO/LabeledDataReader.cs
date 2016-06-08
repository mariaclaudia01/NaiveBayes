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
              
        string Normalize(string posTag)
        {
            if (PartsOfSpeech.Punctuation.Contains(posTag))
                return PartsOfSpeech.EndOfSentence;

            return PartsOfSpeech.Map.ContainsKey(posTag[0])
                ? PartsOfSpeech.Map[posTag[0]]
                : PartsOfSpeech.Unknown;
        }

        private bool IsValid(WordPartOfSpeech trainingItem)
        {
            if (trainingItem.Word == ".") return true;
            if (trainingItem.Word == "_") return false;
            return Regex.IsMatch(trainingItem.Word, @"^[a-z_]*$");
        }
    }
}
