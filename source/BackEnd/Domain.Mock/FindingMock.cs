using System.Collections.Generic;
using Domain.Interface;

namespace Domain.Mock
{
    public class FindingMock : IFinding
    {
        public string TagName { get; set; }
        public Likeliness Likeliness { get; set; }
        public IEnumerable<IPillWarning> PillWarnings { get; set; }
    }
}
