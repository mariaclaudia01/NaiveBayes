using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace NaiveBayes
{
    class TrainingSetReader
    {
        public IEnumerable<WordPartOfSpeech> Read(string filename)
        {
            var wordXmlTags = ReadWordXmlTags(filename);
            int trainingItemsCount = (int)(wordXmlTags.Count() * 0.7);
            return wordXmlTags
                .Select(Parse)
                .Where(wordPartOfSpeech => IsValid(wordPartOfSpeech.Word))
                .Take(trainingItemsCount);
        }

        private IEnumerable<XElement> ReadWordXmlTags(string filename)
        {
            return XDocument.Load(filename)
               .XPathSelectElements("/text/sentence/word");
        }

        private WordPartOfSpeech Parse(XElement wordXmlTag)
        {
            return new WordPartOfSpeech
            {
                Word = wordXmlTag.Attribute("lemma").Value.ToLower(),
                PartOfSpeech = wordXmlTag.Attribute("postag").Value[0].ToString().ToLower()
            };
        }
        private bool IsValid(string word)
        {
            return Regex.IsMatch(word, @"^[a-z_]+$");
        }
    }
}
