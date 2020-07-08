using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DrugCheckingCrawler.Parsers
{
    public class Parser
    {
        private readonly ILogger<Parser> _logger;

        public Parser(ILogger<Parser> logger)
        {
            _logger = logger;
        }

        public ParserResult ParseFile(byte[] fileContent)
        {
            PdfDocument document;
            try
            {
                document = new PdfDocument(new PdfReader(new MemoryStream(fileContent)));
            }
            catch (iText.IO.IOException)
            {
                _logger.LogWarning("Content is not a valid pdf");
                return null;
            }

            var allPages = new List<PdfPage>();
            for (var i = 1; i <= document.GetNumberOfPages(); i++)
            {
                allPages.Add(document.GetPage(i));
            }

            var text = ExtractText(allPages);

            var textParser = new SectionParser(text);
            textParser.Parse();

            if (!textParser.Success)
                return null;

            var preparer = new DetailParser(textParser);
            preparer.Parse();

            if (!preparer.Success)
                return null;

            var image = ExtractImages(allPages.First());
            if (image == null)
                return null;

            return new ParserResult(preparer, image);
        }

        private static string ExtractText(List<PdfPage> allPages)
        {
            return string.Join(ParserConstants.NewLine, allPages.Select(PdfTextExtractor.GetTextFromPage));
        }

        private byte[] ExtractImages(PdfPage pdfPage)
        {
            var imageExtractor = new ImageExtractor();
            var pdfCanvasProcessor = new PdfCanvasProcessor(imageExtractor);
            pdfCanvasProcessor.ProcessPageContent(pdfPage);

            if (imageExtractor.Result.Count() == 1)
            {
                return imageExtractor.Result.First();
            }

            _logger.LogWarning("File does not contain an image");
            return null;
        }
    }
}
