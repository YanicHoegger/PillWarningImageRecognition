using DatabaseInteraction.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DatabaseInteraction
{
    public class CachedRepositoryFactory : RepositoryFactoryBase
    {
        private readonly Dictionary<Type, object> _cach = new Dictionary<Type, object>();

        public CachedRepositoryFactory(IContext context, ILogger<RepositoryFactory> logger) 
            : base(context, logger)
        {
        }

        protected override IRepository<T> OnCreate<T>()
        {
            if(_cach.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }

            var newCreated = new CachedRepository<T>(Container);
            _cach[typeof(T)] = newCreated;

            return newCreated;
        }
    }
}
