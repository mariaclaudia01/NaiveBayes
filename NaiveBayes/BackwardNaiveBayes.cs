using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes
{
    public class BackwardNaiveBayes: IPredictor
    {
        public void Train(IEnumerable<WordPartOfSpeech> trainingSet)
        {
            
        }

        public string PredictPartOfSpeech(string word)
        {
            return "";
        }
    }
}
