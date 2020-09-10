using System.Collections.Generic;

namespace Clients.Shared
{
    public class Finding
    {
        public string TagName { get; set; }
        public Likeliness Likeliness { get; set; }
        public List<PillWarning> PillWarnings { get; set; }
    }
}
