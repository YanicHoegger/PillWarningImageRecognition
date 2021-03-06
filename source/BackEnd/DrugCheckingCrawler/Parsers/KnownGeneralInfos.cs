﻿using System.Collections.Generic;
using System.Linq;

namespace DrugCheckingCrawler.Parsers
{
    public static class KnownGeneralInfos
    {
        public static readonly List<string> List;

        public static readonly List<string> PossibleLongContents = new List<string>
        {
            "Inhaltsstoffe",
            "Inhaltsstoff",
        };

        private static readonly List<string> _shortContents = new List<string>
        {
            "Name",
            "Gewicht",
            "Durchmesser",
            "Dicke",
            "Bruchrille",
            "Farbe",
            "Getestet in",
            "Länge x Breite"
        };

        static KnownGeneralInfos()
        {
            List = _shortContents
            .Concat(PossibleLongContents)
            .ToList();
        }
    }
}
