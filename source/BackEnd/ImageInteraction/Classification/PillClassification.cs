﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Interface;

namespace ImageInteraction.Classification
{
    public class PillClassification : IClassifier
    {
        private readonly IPillClassificationCommunication _pillClassificationCommunication;

        public PillClassification(IPillClassificationCommunication pillClassificationCommunication)
        {
            _pillClassificationCommunication = pillClassificationCommunication;
        }

        public async Task<IEnumerable<IClassificationResult>> GetImageClassification(byte[] image)
        {
            var result = await _pillClassificationCommunication.ClassifyImage(new MemoryStream(image));

            return result.Predictions.Select(x => new ClassificationResult(x.TagName, x.Probability));
        }
    }
}
