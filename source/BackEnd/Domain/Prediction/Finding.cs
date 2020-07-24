using System.Collections.Generic;
using Domain.Interface;

namespace Domain.Prediction
{
    public class Finding : IFinding
    {
        public Finding(string tagName, Likeliness likeliness, IEnumerable<IPillWarning> pillWarnings)
        {
            TagName = tagName;
            Likeliness = likeliness;
            PillWarnings = pillWarnings;
        }

        public string TagName { get; }
        public Likeliness Likeliness { get; }
        public IEnumerable<IPillWarning> PillWarnings { get; }
    }
}
