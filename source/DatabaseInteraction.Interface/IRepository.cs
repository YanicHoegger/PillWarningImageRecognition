using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseInteraction.Interface
{
    public interface IRepository<T> where T : Entity, new()
    {
        Task<List<T>> Find(string queryDefinition);
        Task<List<T>> Get();
        Task Insert(T entity);
        Task Insert(IEnumerable<T> entities);
    }
}
