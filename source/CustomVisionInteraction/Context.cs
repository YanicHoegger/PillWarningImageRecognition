using System;

namespace CustomVisionInteraction
{
    public class Context : IContext
    {
        public Context(string key, string endPoint, Guid projectId)
        {
            Key = key;
            EndPoint = endPoint;
            ProjectId = projectId;
        }

        public string Key { get; }

        public string EndPoint { get; }

        public Guid ProjectId { get; }
    }
}
