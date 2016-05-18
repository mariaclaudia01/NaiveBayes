using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaiveBayes;

namespace NaiveBayesTests
{
    [TestClass]
    public class BasicGlobalProbabilityTests
    {
        BasicGlobalProbability basicGlobalProbability;

        [TestInitialize]
        public void Setup()
        {
            basicGlobalProbability = new BasicGlobalProbability();
        }

        [TestMethod]
        public void TrainingSetContainsOneWordAsTwoDifferentEquallyLikelyPartsOfSpeech()
        {
            var trainingSet = new List<WordPartOfSpeech>
            {
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "x"},
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "y"}
            };

            basicGlobalProbability.Train(trainingSet);

            var actual = basicGlobalProbability.Probabilities("a");

            var expected = new List<PartOfSpeechProbability>
            {
                new PartOfSpeechProbability{PartOfSpeech = "x", Probability = .5},
                new PartOfSpeechProbability{PartOfSpeech = "y", Probability = .5}
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrainingSetContainsOneWordAsTwoDifferentPartsOfSpeechOneMoreLikelyThanTheOther()
        {
            var trainingSet = new List<WordPartOfSpeech>
            {
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "x"},
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "x"},
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "x"},
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "y"}
            };

            basicGlobalProbability.Train(trainingSet);

            var actual = basicGlobalProbability.Probabilities("a");

            var expected = new List<PartOfSpeechProbability>
            {
                new PartOfSpeechProbability{PartOfSpeech = "x", Probability = .75},
                new PartOfSpeechProbability{PartOfSpeech = "y", Probability = .25}
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrainingSetContainsTwoWordsEachAppearingAsASinglePartOfSpeech()
        {
            var trainingSet = new List<WordPartOfSpeech>
            {
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "x"},
                new WordPartOfSpeech {Word = "b", PartOfSpeech = "y"}
            };

            basicGlobalProbability.Train(trainingSet);

            var actual = basicGlobalProbability.Probabilities("a");

            var expected = new List<PartOfSpeechProbability>
            {
                new PartOfSpeechProbability{PartOfSpeech = "x", Probability = 1}
            };

            CollectionAssert.AreEqual(expected, actual);

            actual = basicGlobalProbability.Probabilities("b");

            expected = new List<PartOfSpeechProbability>
            {
                new PartOfSpeechProbability{PartOfSpeech = "y", Probability = 1}
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrainingSetDoesNotContainAnyWord()
        {
            var emptyTrainingSet = new List<WordPartOfSpeech>();

            basicGlobalProbability.Train(emptyTrainingSet);

            var actual = basicGlobalProbability.Probabilities("a");

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TrainingSetContainsOnePartOfSpeechThatAppearsTwice()
        {
            var trainingSet = new List<WordPartOfSpeech>()
             {
                new WordPartOfSpeech {Word = "a", PartOfSpeech = "x"},
                new WordPartOfSpeech {Word = "b", PartOfSpeech = "x"}
            };

            basicGlobalProbability.Train(trainingSet);

            Assert.AreEqual(2, basicGlobalProbability.PartOfSpeechStatistics["x"]);
        }
    }
}
