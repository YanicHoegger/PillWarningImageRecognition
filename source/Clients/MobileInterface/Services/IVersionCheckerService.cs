using System;

namespace MobileInterface.Services
{
    public interface IVersionCheckerService
    {
        bool IsVersionChecked { get; }
        bool IsVersionCorrect { get; }

        event Action VersionChecked;
    }
}
