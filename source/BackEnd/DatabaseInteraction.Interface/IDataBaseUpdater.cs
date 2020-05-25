using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseInteraction.Interface
{
    public interface IDataBaseUpdater
    {
        Task Update<T>(IRepositoryFactory repositoryFactory, IEnumerable<T> toUpdate, Func<T, bool> comparer) where T : Entity, new();
    }
}
