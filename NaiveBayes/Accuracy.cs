using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes
{
    public class Accuracy
    {
        IPredictor predictor;

        public Accuracy(IPredictor predictor)
        {
            this.predictor = predictor;
        }

        public double Compute(IEnumerable<WordPartOfSpeech> testSet)
        {
            int successfulPredictions = testSet
                .Where(IsSuccessfulPrediction)
                .Count();

            return (double)successfulPredictions / testSet.Count();
        }

        private bool IsSuccessfulPrediction(WordPartOfSpeech testItem)
        {
            return predictor.PredictPartOfSpeech(testItem.Word) == testItem.PartOfSpeech;
        }
    }
}
