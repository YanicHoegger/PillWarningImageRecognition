using System;
using System.Threading.Tasks;

namespace DatabaseInteraction.Interface
{
    public interface IDataBaseUpdater
    {
        Task Update<T>(IRepository<T> repository, T toUpdate, Func<T, bool> predicate) where T : Entity, new();
    }
}
