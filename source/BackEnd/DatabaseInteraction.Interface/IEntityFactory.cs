namespace DatabaseInteraction.Interface
{
    public interface IEntityFactory
    {
        T Create<T>() where T : Entity, new();
    }
}
