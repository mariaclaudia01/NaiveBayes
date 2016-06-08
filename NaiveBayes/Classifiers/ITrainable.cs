using System.Collections.Generic;

namespace NaiveBayes.Classifiers
{
    public interface ITrainable
    {
        void Train(List<WordPartOfSpeech> trainingSet);
        double Accuracy(List<WordPartOfSpeech> testSet);
        double Precision(string partOfSpeech, List<WordPartOfSpeech> testSet);
        double Recall(string partOfSpeech, List<WordPartOfSpeech> testSet);        
    }
}
