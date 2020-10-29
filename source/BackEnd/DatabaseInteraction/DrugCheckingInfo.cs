using DatabaseInteraction.Interface;

namespace DatabaseInteraction
{
    public class DrugCheckingInfo : Entity.Entity, IDrugCheckingInfo
    {
        public string Title { get; set; }
        public string Info { get; set; }
    }
}
