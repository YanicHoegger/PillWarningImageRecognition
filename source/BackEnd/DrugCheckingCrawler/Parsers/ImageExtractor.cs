using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Collections.Generic;

namespace DrugCheckingCrawler.Parsers
{
    public class ImageExtractor : IEventListener
    {
        private readonly List<byte[]> _imageContents = new List<byte[]>();

        public void EventOccurred(IEventData data, EventType type)
        {
            if (type != EventType.RENDER_IMAGE)
                return;

            ImageRenderInfo imageRenderInfo = (ImageRenderInfo)data;

            _imageContents.Add(imageRenderInfo.GetImage().GetImageBytes());
        }

        public ICollection<EventType> GetSupportedEvents() => null;

        public IEnumerable<byte[]> Result => _imageContents;
    }
}
