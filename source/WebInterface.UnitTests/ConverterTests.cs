using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Domain.Interface;
using Domain.Prediction;
using WebInterface.Services;
using PredictionResult = Clients.Shared.PredictionResult;

namespace WebInterface.UnitTests
{
    [TestFixture]
    public class ConverterTests
    {
        [Test]
        public void ConvertCorrectTest()
        {
            GivenDrugCheckingSource();
            WhenConvert();
            ThenCorrectConverted();
        }

        private IPredictionResult _predictionResult;
        private PredictionResult _pillWarning;

        private void GivenDrugCheckingSource()
        {
            var fixture = new Fixture();

            var tagFindings = new List<Finding>
            {
                new Finding("TagName", Likeliness.Very, new List<PillWarning>
                {
                    fixture.Create<PillWarning>(),
                    fixture.Create<PillWarning>(),
                    fixture.Create<PillWarning>()
                })
            };

            var colorFindings = new List<PillWarning>
            {
                fixture.Create<PillWarning>(),
                fixture.Create<PillWarning>()
            };

            _predictionResult = new Domain.Prediction.PredictionResult(Likeliness.Sure, tagFindings, colorFindings);
        }

        private void WhenConvert()
        {
            _pillWarning = Converter.ToPredictionResult(_predictionResult);
        }

        private void ThenCorrectConverted()
        {
            CheckProperties(_predictionResult, _pillWarning);
        }

        private static void CheckProperties(object source, object destination)
        {
            if (source.GetType() == destination.GetType())
            {
                Assert.AreEqual(source, destination);
                return;
            }

            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            foreach (var destinationProperty in destinationProperties)
            {
                var destinationValue = destinationProperty.GetValue(destination);
                var sourceValue = sourceProperties
                    .Single(x => x.Name.Equals(destinationProperty.Name))
                    .GetValue(source);

                // ReSharper disable PossibleNullReferenceException : We checked for type before
                // ReSharper disable AssignNullToNotNullAttribute
                if (typeof(IEnumerable<object>).IsAssignableFrom(destinationProperty.PropertyType))
                {
                    var sourceListValue = ((IEnumerable<object>)sourceValue).ToList();
                    var destinationListValue = ((IEnumerable<object>)destinationValue).ToList();

                    Assert.AreEqual(sourceListValue.Count, destinationListValue.Count);

                    for (int i = 0; i < sourceListValue.Count; i++)
                    {
                        CheckProperties(sourceListValue[i], destinationListValue[i]);
                    }
                }
                else if (destinationProperty.PropertyType.IsEnum)
                {
                    Assert.AreEqual((int)sourceValue, (int)destinationValue, $"Property '{destinationProperty.Name}' is incorrect");
                }
                else
                {
                    Assert.AreEqual(sourceValue, destinationValue, $"Property '{destinationProperty.Name}' is incorrect");
                }
                // ReSharper restore PossibleNullReferenceException
                // ReSharper restore AssignNullToNotNullAttribute
            }
        }
    }
}
