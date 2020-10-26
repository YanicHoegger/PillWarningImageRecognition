using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseInteraction.Interface
{
    public interface IRepository<T> where T : Entity, new()
    {
        //TODO: IAsyncEnumerable
        Task<List<T>> Get();
        Task Insert(T entity);
        Task Insert(IEnumerable<T> entities);
        Task Update(T toUpdate, Guid id);
    }
}
