﻿using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DrugCheckingCrawler
{
    public class Parser
    {
        private const string Name = "name";
        private const string Colors = "colors";
        private const string Date = "date";
        private static readonly string RegexPattern = @$"(?<{Date}>(\w+ [0-9]+)).*Name (?<{Name}>[\w \.]+).*Farbe (?<{Colors}>[\w, ]+).*";

        public ParserResult ParseFile(byte[] fileContent)
        {
            var document = new PdfDocument(new PdfReader(new MemoryStream(fileContent)));
            var firstPage = document.GetPage(1);

            var text = ExtractText(firstPage);
            (bool match, string name, IEnumerable<string> colors, DateTime creation) = ParseText(text);

            if (!match)
                return null;

            return new ParserResult(name, colors, creation, ExtractImages(firstPage));
        }

        private static (bool match, string name, IEnumerable<string> colors, DateTime creation) ParseText(string input)
        {
            var match = Regex.Match(input, RegexPattern, RegexOptions.Singleline);

            if(!match.Success)
            {
                return (false, string.Empty, Enumerable.Empty<string>(), DateTime.MinValue);
            }

            var colors = match.Groups[Colors].Value.Split(", ");
            var date = DateTime.Parse(match.Groups[Date].Value, CultureInfo.GetCultureInfo("de-CH"));

            return (true, match.Groups[Name].Value, colors, date);
        }

        private static string ExtractText(PdfPage pdfPage)
        {
            return PdfTextExtractor.GetTextFromPage(pdfPage);
        }

        private static byte[] ExtractImages(PdfPage pdfPage)
        {
            var imageExtractor = new ImageExtractor();
            var pdfCanvasProcessor = new PdfCanvasProcessor(imageExtractor);
            pdfCanvasProcessor.ProcessPageContent(pdfPage);

            //TODO: Remove when run through once
            Debug.Assert(imageExtractor.Result.Count() == 1);

            return imageExtractor.Result.First();
        }
    }
}
