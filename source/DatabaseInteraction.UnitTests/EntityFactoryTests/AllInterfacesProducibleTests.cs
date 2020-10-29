using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseInteraction.Entity;
using NUnit.Framework;
using Telerik.JustMock;

namespace DatabaseInteraction.UnitTests.EntityFactoryTests
{
    public class AllInterfacesProducibleTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void InterfaceProducibleTests(Type @interface, Type implementation, IEnumerable<EntityMapping> entityMappings)
        {
            GivenInterfaceEntity(@interface);
            WhenProduceImplementation(@interface, implementation, entityMappings);
            ThenNoException();
        }

        private object _interfaceMock;

        private void GivenInterfaceEntity(Type @interface)
        {
            _interfaceMock = Mock.Create(@interface);
        }

        private void WhenProduceImplementation(Type @interface, Type implementation, IEnumerable<EntityMapping> entityMappings)
        {
            var factory = new EntityFactory(entityMappings);

            var method = typeof(EntityFactory).GetMethod(nameof(EntityFactory.CreateImplementation));
            // ReSharper disable once PossibleNullReferenceException : Is ok for test
            var genericMethod = method.MakeGenericMethod(@interface, implementation);
            genericMethod.Invoke(factory, new[] { _interfaceMock });
        }

        private static void ThenNoException()
        {
            //Nothing to do here
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            var implementations = EntityTypesHelper.GetImplementations();

            var typeMappings = EntityTypesHelper.GetInterfaces()
                .Select(x => new KeyValuePair<Type, Type>(x, GetBestImplementation(implementations, x))).ToList();

            var entityMappings = typeMappings.Select(x => CreateEntityMapping(x.Key, x.Value));

            foreach ((Type key, Type value) in typeMappings)
            {
                yield return new TestCaseData(key, value, entityMappings);
            }
        }

        private static Type GetBestImplementation(IEnumerable<Type> implementations, Type x)
        {
            return implementations.Where(x.IsAssignableFrom)
                .OrderBy(y => y.GetInterfaces().Length)
                .First();
        }

        private static EntityMapping CreateEntityMapping(Type @interface, Type implementation)
        {
            var genericType = typeof(EntityMapping.TypedEntityMapping<,>).MakeGenericType(@interface, implementation);
            return (EntityMapping)Activator.CreateInstance(genericType);
        }
    }
}
