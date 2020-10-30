using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction.Repository
{
    // ReSharper disable once UnusedTypeParameter : Is used for DI
    public interface IContainerFactory<T>
    {
        Container Container { get; }
    }
}
