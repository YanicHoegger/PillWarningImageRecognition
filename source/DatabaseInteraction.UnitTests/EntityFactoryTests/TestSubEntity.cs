using System.Collections.Generic;

namespace DatabaseInteraction.UnitTests.EntityFactoryTests
{
    public class TestSubEntity : Entity.Entity, ITestSubEntity
    {
        public string SomeValue { get; set; }
        public IEnumerable<int> SomeMoreValues { get; set; }
    }
}
