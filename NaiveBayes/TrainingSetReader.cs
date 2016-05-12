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
            var words = ReadWordXmlTags(filename)
                .Select(Parse)
                .Where(IsValid)
                .ToList();

            int trainingItemsCount = (int)(words.Count() * 0.7);

            return words.Take(trainingItemsCount);
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
                PartOfSpeech = Normalize(wordXmlTag.Attribute("postag").Value.ToLower())
            };
        }

        Dictionary<char, string> map = new Dictionary<char, string>
        {
            	{'t', "articol"},
                {'n', "subst"},
                {'a', "adj"},
                {'v', "verb"},
                {'q', "conj"},
                {'p', "pron"},
                {'r', "adv"},
                {'c', "conj"},
                {'d', "pron"},
                {'i', "interj"}, 
                {'m', "numeral"},
        };

        string Normalize(string posTag)
        {
            return map.ContainsKey(posTag[0])
                ? map[posTag[0]]:
                "altele";
        }

        private bool IsValid(WordPartOfSpeech trainingItem)
        {
            return Regex.IsMatch(trainingItem.Word, @"^[a-z_]*[a-z]+$");
        }
    }
}
