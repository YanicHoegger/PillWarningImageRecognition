﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace ImageInteraction.Training
{
    public interface ITrainerCommunicator
    {
        bool IsInitialized { get; }

        Task AddImage(Stream imageStream, IEnumerable<Tag> tags);
        Task<Tag> CreateTag(string name);
        IEnumerable<Task<byte[]>> DownloadImages();
        Tag GetTag(string tagName);
    }
}
