using System.Collections.Generic;

namespace Clients.Shared
{
    public class PredictionResult
    {
        public Likeliness IsPill { get; set; }
        public List<Finding> TagFindings { get; set; }
        public List<PillWarning> ColorFindings { get; set; }
    }
}
