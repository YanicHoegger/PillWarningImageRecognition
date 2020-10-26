using System.Collections.Generic;
using ImageInteraction.Interface;

namespace ImageInteraction.Training
{
    public class TrainedImagesManager : ITrainedImagesManager
    {
        private readonly ITrainerCommunicator _trainerCommunicator;

        public TrainedImagesManager(ITrainerCommunicator trainerCommunicator)
        {
            _trainerCommunicator = trainerCommunicator;
        }

        public IAsyncEnumerable<byte[]> GetTrainedImages()
        {
            return _trainerCommunicator.DownloadImages();
        }
    }
}
