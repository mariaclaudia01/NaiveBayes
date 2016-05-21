using System.Collections.Generic;

namespace NaiveBayes
{
    public interface ITrainable
    {
        void Train(List<WordPartOfSpeech> trainingSet);
        double Accuracy(List<WordPartOfSpeech> testSet);
    }
}
