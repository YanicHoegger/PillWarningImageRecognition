using System.Collections.Generic;

namespace Domain.Interface
{
    public interface IFinding
    {
        string TagName { get; }
        Likeliness Likeliness { get; }

        IEnumerable<IPillWarning> PillWarnings { get; }
    }
}
