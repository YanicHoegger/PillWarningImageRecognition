using System;

namespace ImageInteraction
{
    public interface IContext
    {
        string Key { get; }
        string EndPoint { get; }
        Guid ProjectId { get; }
    }
}
