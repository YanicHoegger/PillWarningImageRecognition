using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseInteraction.Interface
{
    public interface IRepository<T> where T : Entity, new()
    {
        IAsyncEnumerable<T> Get();

        Task Insert(T entity);
        Task Update(T toUpdate, Guid id);
    }
}
