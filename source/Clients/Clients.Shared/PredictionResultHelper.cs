using System;
using System.Collections.Generic;

namespace Clients.Shared
{
    public class PredictionResultHelper
    {
        private readonly Dictionary<Likeliness, string> _likelinessDictionary = new Dictionary<Likeliness, string>
        {
            { Likeliness.Maybe, "Eher unwahrscheinlich aber könnte sein das es diese Pillen sind" },
            { Likeliness.Very, "Gut möglich dass es diese Pillen sind" },
            { Likeliness.Sure, "Sehr gut möglich dass es diese Pillen sind" },
        };

        public bool IsPill(PredictionResult predictionResult) => predictionResult.IsPill > Likeliness.Very;

        public string NoPillResponse
        {
            get => "Ooops das war wohl nicht das beste Bild. Wir glauben nicht dass das hochgeladene Bild eine Pille ist";
        }

        public string ConvertLikeliness(Likeliness likeliness)
        {
            if (_likelinessDictionary.TryGetValue(likeliness, out var value))
            {
                return value;
            }

            throw new ArgumentOutOfRangeException(nameof(likeliness), likeliness, "Can not convert likeliness");
        }
    }
}
