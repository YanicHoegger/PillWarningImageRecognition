using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Drawing;

namespace DatabaseInteraction
{
    public class DrugCheckingSource : Entity
    {
        public string Name { get; set; }

        [BsonSerializer(typeof(ColorSerializer))]
        public Color Color { get; set; }

        public DateTime Creation { get; set; }

        public string PdfLocation { get; set; }

        public byte[] Image { get; set; }

        public string DocumentHash { get; set; }
    }
}
