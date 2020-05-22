using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DrugCheckingCrawler
{
    public class Parser
    {
        private const string _name = "name";
        private const string _date = "date";
        private static readonly string _regexPattern = @$"\n(?<{_date}>(\w+ [0-9]+))\nName (?<{_name}>[\w \.]+)";

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

            var firstPage = document.GetPage(1);

            var text = ExtractText(firstPage);
            (bool match, string name, DateTime creation) = ParseText(text);

            if (!match)
                return null;

            var image = ExtractImages(firstPage);
            if (image == null)
                return null;

            return new ParserResult(name, creation, image);
        }

        private static (bool match, string name, DateTime creation) ParseText(string input)
        {
            var match = Regex.Match(input, _regexPattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                return (false, string.Empty, DateTime.MinValue);
            }

            var date = DateTime.Parse(match.Groups[_date].Value, CultureInfo.GetCultureInfo("de-CH"));

            return (true, match.Groups[_name].Value, date);
        }

        private static string ExtractText(PdfPage pdfPage)
        {
            return PdfTextExtractor.GetTextFromPage(pdfPage);
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
