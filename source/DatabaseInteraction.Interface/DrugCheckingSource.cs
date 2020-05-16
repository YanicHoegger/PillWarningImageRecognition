using System;
using System.Drawing;
using System.Text.Json.Serialization;
using Utilities;

namespace DatabaseInteraction.Interface
{
    public class DrugCheckingSource : Entity
    {
        public string Name { get; set; }

        [JsonConverter(typeof(ColorSerializer))]
        public Color Color { get; set; }

        public DateTime Creation { get; set; }

        public string PdfLocation { get; set; }

        public byte[] Image { get; set; }

        public string DocumentHash { get; set; }
    }
}
