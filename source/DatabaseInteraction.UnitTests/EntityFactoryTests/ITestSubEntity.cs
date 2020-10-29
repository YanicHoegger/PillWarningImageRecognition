using System.Collections.Generic;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.UnitTests.EntityFactoryTests
{
    public interface ITestSubEntity : IEntity
    {
        public string SomeValue { get; set; }
        public IEnumerable<int> SomeMoreValues { get; set; }
    }
}
