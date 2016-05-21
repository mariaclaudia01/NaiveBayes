﻿using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public class BackwardNaiveBayes
    {
        private readonly BasicGlobalProbability basicGlobalProbability = new BasicGlobalProbability();

        public Dictionary<string, Dictionary<string, Dictionary<string, int>>> Statistics = 
            new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

        public void Train(List<WordPartOfSpeech> trainingSet)
        {
            basicGlobalProbability.Train(trainingSet);

            var predecessor = trainingSet.FirstOrDefault();

            foreach (var item in trainingSet.Skip(1))
            {
                Train(item, predecessor);
                predecessor = item;
            }
        }

        private void Train(WordPartOfSpeech current, WordPartOfSpeech predecessor)
        {
            Statistics
                .GetOrCreate(current.Word)
                .GetOrCreate(predecessor.PartOfSpeech)
                .GetOrCreate(current.PartOfSpeech);

            Statistics[current.Word][predecessor.PartOfSpeech][current.PartOfSpeech]++;
        }

        public Dictionary<string, double> Probabilities(string word, string predecessorPartOfSpeech)
        {
            return Statistics.GetOrCreate(word).GetOrCreate(predecessorPartOfSpeech).Statistics();
        }

        public string PredictPartOfSpeech(string word, string predecessorPartOfSpeech)
        {
            var partOfSpeechProbabilities = Probabilities(word, predecessorPartOfSpeech);

            if (partOfSpeechProbabilities.Any())
            {
                return partOfSpeechProbabilities.KeyWithMaxValue();
            }
            return basicGlobalProbability.PredictPartOfSpeech(word);
        }
    }
}
