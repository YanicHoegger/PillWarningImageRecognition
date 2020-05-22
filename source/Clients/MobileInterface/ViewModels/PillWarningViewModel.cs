using System.IO;

namespace MobileInterface.ViewModels
{
    public class PillWarningViewModel
    {
        public PillWarningViewModel(Stream contentStream)
        {
            ContentStream = contentStream;
        }

        public Stream ContentStream { get; }
    }
}
