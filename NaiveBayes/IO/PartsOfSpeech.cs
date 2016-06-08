using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes.IO
{
    public class PartsOfSpeech
    {
        public static readonly Dictionary<char, string> Map = new Dictionary<char, string>
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

        public static readonly List<string> Punctuation = new List<string>
        {
           "punct","punct.","PERIOD", "."
        };

        public const string EndOfSentence = "punct";
        public const string Unknown = "altele";
    }
}
