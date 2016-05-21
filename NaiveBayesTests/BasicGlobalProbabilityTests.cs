using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaiveBayes;
using NaiveBayes.Classifiers;

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

            var expected = new Dictionary<string, double>
            {
                {"x", 0.5},
                {"y", 0.5}
            };

            CollectionAssert.AreEquivalent(expected, actual);
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

            var expected = new Dictionary<string, double>
            {
                {"x", 0.75},
                {"y", 0.25}
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

            var expected = new Dictionary<string, double>
            {
                {"x", 1}
            };

            CollectionAssert.AreEqual(expected, actual);

            actual = basicGlobalProbability.Probabilities("b");

            expected = new Dictionary<string, double>
            {
                {"y", 1}
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
