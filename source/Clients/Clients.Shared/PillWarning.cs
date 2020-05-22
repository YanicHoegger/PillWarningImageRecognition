using System;
using System.Drawing;
using System.Text.Json.Serialization;
using Utilities;

namespace Clients.Shared
{
    public class PillWarning
    {
        public string Name { get; set; }

        [JsonConverter(typeof(ColorSerializer))]
        public Color Color { get; set; }

        public DateTime Creation { get; set; }

        public string PdfLocation { get; set; }

        public byte[] Image { get; set; }
    }
}
