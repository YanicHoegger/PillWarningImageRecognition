using AutoFixture.NUnit3;
using Clients.Shared;
using DatabaseInteraction.Interface;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WebInterface.Services;

namespace WebInterface.UnitTests
{
    [TestFixture]
    public class ConverterTests
    {
        [Test, AutoData]
        public void ConvertCorrectTest(DrugCheckingSource drugCheckingSource)
        {
            GivenDrugCheckingSource(drugCheckingSource);
            WhenConvert();
            ThenCorrectConverted();
        }

        private DrugCheckingSource _drugCheckingSource;
        private PillWarning _pillWarning;

        private void GivenDrugCheckingSource(DrugCheckingSource drugCheckingSource)
        {
            _drugCheckingSource = drugCheckingSource;
        }

        private void WhenConvert()
        {
            _pillWarning = Converter.ToPillWarning(_drugCheckingSource);
        }

        private void ThenCorrectConverted()
        {
            AssertProperties(_drugCheckingSource, _pillWarning);
        }

        private static void AssertProperties(object source, object destination)
        {
            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            foreach (var destinationProperty in destinationProperties)
            {
                var destinationValue = destinationProperty.GetValue(destination);
                var sourceValue = sourceProperties
                    .Single(x => x.Name.Equals(destinationProperty.Name))
                    .GetValue(source);

                if(destinationProperty.PropertyType.IsAssignableFrom(typeof(List<PillWarningInfo>)))
                {
                    var sourceList = (List<DrugCheckingInfo>)sourceValue;
                    var destinationList = (List<PillWarningInfo>)destinationValue;

                    Assert.AreEqual(sourceList.Count, destinationList.Count);

                    for(var i = 0; i < sourceList.Count; i++)
                    {
                        AssertProperties(sourceList[i], destinationList[i]);
                    }
                }
                else
                {
                    Assert.AreEqual(sourceValue, destinationValue, $"Property '{destinationProperty.Name}' is incorrect");
                }
            }
        }
    }
}
