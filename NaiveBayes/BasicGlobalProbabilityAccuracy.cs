using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BasicGlobalProbabilityAccuracy
    {
        readonly BasicGlobalProbability predictor;

        public BasicGlobalProbabilityAccuracy(BasicGlobalProbability predictor)
        {
            this.predictor = predictor;
        }

        public double Compute(List<WordPartOfSpeech> testSet)
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
