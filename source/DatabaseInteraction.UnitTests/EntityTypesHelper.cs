using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.UnitTests
{
    public static class EntityTypesHelper
    {
        public static IEnumerable<Type> GetInterfaces()
        {
            var entityInterfaceType = typeof(IEntity);
            return entityInterfaceType.Assembly.GetTypes().Where(x => entityInterfaceType.IsAssignableFrom(x) && x != entityInterfaceType);
        }

        public static IEnumerable<Type> GetImplementations()
        {
            return typeof(Entity.Entity).Assembly.GetTypes()
                .Where(x => typeof(IEntity).IsAssignableFrom(x)).ToList();
        }
    }
}
