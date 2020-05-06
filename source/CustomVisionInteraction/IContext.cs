﻿using System;

namespace CustomVisionInteraction
{
    public interface IContext
    {
        string Key { get; }
        string EndPoint { get; }
        Guid ProjectId { get; }
    }
}
