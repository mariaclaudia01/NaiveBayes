using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes
{
    public interface IPredictor
    {
        void Train(IEnumerable<WordPartOfSpeech> trainingSet);
        string PredictPartOfSpeech(string word);        
    }
}
