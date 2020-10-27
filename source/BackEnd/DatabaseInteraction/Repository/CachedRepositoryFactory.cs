using System;
using System.Collections.Generic;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction.Repository
{
    public class CachedRepositoryFactory : IRepositoryFactory
    {
        private readonly Container _container;
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        private CachedDrugCheckingSourceRepository _drugCheckingSourceRepository;

        public CachedRepositoryFactory(Container container)
        {
            _container = container;
        }

        public IRepository<T> Create<T>() where T : Entity, new()
        {
            if (_cache.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }

            var newCreated = new CachedRepository<T>(_container);
            _cache[typeof(T)] = newCreated;

            return newCreated;
        }

        public IDrugCheckingSourceRepository CreateDrugCheckingSourceRepository()
        {
            _drugCheckingSourceRepository ??= new CachedDrugCheckingSourceRepository(_container);

            return _drugCheckingSourceRepository;
        }
    }
}
