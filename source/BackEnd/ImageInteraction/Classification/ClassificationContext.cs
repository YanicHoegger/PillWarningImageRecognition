using System;

namespace ImageInteraction.Classification
{
    // ReSharper disable once UnusedMember.Global : Can be used for manipulation and experiments
    public class ClassificationContext : Context, IClassificationContext
    {
        public ClassificationContext(string key, string endPoint, Guid projectId, string publisherModelName)
            : base(key, endPoint, projectId)
        {
            PublisherModelName = publisherModelName;
        }

        public string PublisherModelName { get; }
    }
}
