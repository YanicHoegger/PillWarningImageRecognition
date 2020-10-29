using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DatabaseInteraction.UnitTests
{
    public class EntityImplementationTests
    {
        [Test]
        public void AllInterfacesImplementedTest()
        {
            GivenInterfaces();
            WhenSearchImplementations();
            ThenAllInterfacesImplemented();
        }

        private IEnumerable<Type> _allInterfaces;
        private IList<Type> _allImplementations;

        private void GivenInterfaces()
        {
            _allInterfaces = EntityTypesHelper.GetInterfaces();
        }

        private void WhenSearchImplementations()
        {
            _allImplementations = EntityTypesHelper.GetImplementations().ToList();
        }

        private void ThenAllInterfacesImplemented()
        {
            foreach (var @interface in _allInterfaces)
            {
                Assert.That(_allImplementations.Count(x => @interface.IsAssignableFrom(x)), Is.GreaterThanOrEqualTo(1));
            }
        }
    }
}
