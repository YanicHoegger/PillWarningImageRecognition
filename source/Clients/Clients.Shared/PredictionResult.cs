using System.Collections.Generic;

namespace Clients.Shared
{
    public class PredictionResult
    {
        public IEnumerable<PillWarning> SameInprint { get; set; } = new List<PillWarning>();
        public IEnumerable<PillWarning> SameColor { get; set; } = new List<PillWarning>();
    }
}
