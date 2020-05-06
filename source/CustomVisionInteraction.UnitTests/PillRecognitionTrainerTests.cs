using FluentAssertions;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionInteraction.UnitTests
{
    [TestFixture]
    public class PillRecognitionTrainerTests
    {
        [Test]
        public async Task SameImageGetFilteredTest()
        {
            GivenExistingImagesAndSameImageToAdd();
            await WhenTrain();
            ThenImageNotImported();
        }

        [Test]
        public async Task SameInputImageGetFilteredTest()
        {
            GivenTwoSameImageToAdd();
            await WhenTrain();
            ThenOneImageImported();
        }

        [TestCase(3,1)]
        [TestCase(1,2)]
        [TestCase(2,2)]
        [TestCase(0,4)]
        public async Task NeedFiveTagsFilteringTest(int existingImageCount, int addingImageCount)
        {
            GivenExistingAndToAddTags(existingImageCount, addingImageCount);
            await WhenTrain();
            ThenImageNotImported();
        }

        [Test]
        public async Task AddFiveImagesTest()
        {
            GivenFiveImagesToImport();
            await WhenTrain();
            ThenFiveImagesImported();
        }

        private TrainerCommunicatorMock _trainerCommunicatorMock;
        private List<(byte[] image, string tag)> _inputData;

        private void GivenExistingImagesAndSameImageToAdd()
        {
            _trainerCommunicatorMock = new TrainerCommunicatorMock
            {
                DownloadingImages = new List<byte[]>
                {
                    ReadImage("Skulls1.jpg"),
                    ReadImage("Skulls2.jpg"),
                    ReadImage("Skulls3.jpg"),
                    ReadImage("Skulls4.jpg"),
                    ReadImage("Skulls5.jpg")
                }
            };

            _inputData = new List<(byte[] image, string tag)>
            {
                (ReadImage("Skulls1.jpg"), "Skull")
            };
        }

        private void GivenTwoSameImageToAdd()
        {
            const string TagName = "Skull";

            _trainerCommunicatorMock = new TrainerCommunicatorMock
            {
                Tags = new List<Tag>
                {
                   new Tag(TagName, string.Empty, string.Empty, default, 5)
                }
            };

            _inputData = new List<(byte[] image, string tag)>
            {
                (ReadImage("Skulls1.jpg"), TagName),
                (ReadImage("Skulls1.jpg"), TagName)
            };
        }

        private void GivenExistingAndToAddTags(int existingImageCount, int addingImageCount)
        {
            const string TagName = "Tag";

            _trainerCommunicatorMock = new TrainerCommunicatorMock
            {
                Tags = new List<Tag>
                {
                   new Tag(TagName, string.Empty, string.Empty, default, existingImageCount)
                }
            };
            _inputData = Enumerable.Range(0, addingImageCount)
                .Select(x => (new byte[0], TagName))
                .ToList();
        }

        private void GivenFiveImagesToImport()
        {
            _trainerCommunicatorMock = new TrainerCommunicatorMock();

            _inputData = new List<(byte[] image, string tag)>
            {
                (ReadImage("Skulls1.jpg"), "Skull"),
                (ReadImage("Skulls2.jpg"), "Skull"),
                (ReadImage("Skulls3.jpg"), "Skull"),
                (ReadImage("Skulls4.jpg"), "Skull"),
                (ReadImage("Skulls5.jpg"), "Skull"),
            };
        }

        private async Task WhenTrain()
        {
            await new PillRecognitionTrainer(_trainerCommunicatorMock).Train(_inputData);
        }

        private void ThenImageNotImported()
        {
            _trainerCommunicatorMock.AddedImage.Should().BeEmpty();
        }

        private void ThenOneImageImported()
        {
            _trainerCommunicatorMock.AddedImage.Count().Should().Be(1);
        }

        private void ThenFiveImagesImported()
        {
            _trainerCommunicatorMock.AddedImage.Count().Should().Be(5);
        }

        private static byte[] ReadImage(string fileName)
        {
            return ImageHelper.ReadImage(Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestFiles", fileName));
        }
    }
}
