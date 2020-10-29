namespace DatabaseInteraction.Interface
{
    public interface IDrugCheckingInfo : IEntity
    {
        string Title { get; set; }
        string Info { get; set; }
    }
}
