using Domain.Interface;
using Domain.Prediction;
using NUnit.Framework;

namespace Domain.UnitTests
{
    [TestFixture]
    public class ProbabilityToLikelinessConverterTest
    {
        [TestCase(0, Likeliness.NotAtAll)]
        [TestCase(1, Likeliness.Sure)]
        [TestCase(0.5, Likeliness.Maybe)]
        [TestCase(0.7, Likeliness.Very)]
        [TestCase(0.9, Likeliness.Sure)]
        public void ConverterTests(double probability, Likeliness expectedLikeliness)
        {
            GivenProbability(probability);
            WhenConvert();
            ThenCorrectResult(expectedLikeliness);
        }

        private double _givenProbability;
        private Likeliness _actualLikeliness;

        private void GivenProbability(double probability)
        {
            _givenProbability = probability;
        }

        private void WhenConvert()
        {
            _actualLikeliness = new ProbabilityToLikelinessConverter().Convert(_givenProbability);
        }

        private void ThenCorrectResult(Likeliness expectedLikeliness)
        {
            Assert.AreEqual(expectedLikeliness, _actualLikeliness);
        }
    }
}
