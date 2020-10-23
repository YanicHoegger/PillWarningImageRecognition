using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ImageInteraction.Interface;
using ImageInteraction.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using NUnit.Framework;
using Telerik.JustMock;

namespace ImageInteraction.UnitTests
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

        [TestCase(3, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(0, 4)]
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
        private List<ITrainingImage> _inputData;

        private void GivenExistingImagesAndSameImageToAdd()
        {
            _trainerCommunicatorMock = new TrainerCommunicatorMock
            {
                DownloadingImages = new List<byte[]>
                {
                    TestHelper.ReadImage("Skulls1.jpg"),
                    TestHelper.ReadImage("Skulls2.jpg"),
                    TestHelper.ReadImage("Skulls3.jpg"),
                    TestHelper.ReadImage("Skulls4.jpg"),
                    TestHelper.ReadImage("Skulls5.jpg")
                }
            };

            _inputData = new List<ITrainingImage>
            {
                CreateTrainingImage("Skulls1.jpg", "Skull")
            };
        }

        private void GivenTwoSameImageToAdd()
        {
            const string tagName = "Skull";

            _trainerCommunicatorMock = new TrainerCommunicatorMock
            {
                Tags = new List<Tag>
                {
                   new Tag(tagName, string.Empty, string.Empty, default, 5)
                }
            };

            _inputData = new List<ITrainingImage>
            {
                CreateTrainingImage("Skulls1.jpg", tagName),
                CreateTrainingImage("Skulls1.jpg", tagName)
            };
        }

        private void GivenExistingAndToAddTags(int existingImageCount, int addingImageCount)
        {
            const string tagName = "Tag";

            _trainerCommunicatorMock = new TrainerCommunicatorMock
            {
                Tags = new List<Tag>
                {
                   new Tag(tagName, string.Empty, string.Empty, default, existingImageCount)
                }
            };
            _inputData = Enumerable.Range(0, addingImageCount)
                .Select(x => CreateTrainingImage(new byte[0], tagName))
                .ToList();
        }

        private void GivenFiveImagesToImport()
        {
            _trainerCommunicatorMock = new TrainerCommunicatorMock();

            _inputData = new List<ITrainingImage>
            {
                CreateTrainingImage("Skulls1.jpg", "Skull"),
                CreateTrainingImage("Skulls2.jpg", "Skull"),
                CreateTrainingImage("Skulls3.jpg", "Skull"),
                CreateTrainingImage("Skulls4.jpg", "Skull"),
                CreateTrainingImage("Skulls5.jpg", "Skull")
            };
        }

        private async Task WhenTrain()
        {
            await new ClassificationTrainer(_trainerCommunicatorMock).Train(_inputData);
        }

        private void ThenImageNotImported()
        {
            _trainerCommunicatorMock.AddedImage.Should().BeEmpty();
        }

        private void ThenOneImageImported()
        {
            _trainerCommunicatorMock.AddedImage.Count.Should().Be(1);
        }

        private void ThenFiveImagesImported()
        {
            _trainerCommunicatorMock.AddedImage.Count.Should().Be(5);
        }

        private static ITrainingImage CreateTrainingImage(string imageName, string tag)
        {
            return CreateTrainingImage(TestHelper.ReadImage(imageName), tag);
        }

        private static ITrainingImage CreateTrainingImage(byte[] image, string tag)
        {
            var trainingImage = Mock.Create<ITrainingImage>();

            Mock.Arrange(() => trainingImage.Image).Returns(image);
            Mock.Arrange(() => trainingImage.Tags).Returns(new[] { tag });

            return trainingImage;
        }
    }
}
