using System.Collections.Generic;

namespace DatabaseInteraction.UnitTests.EntityFactoryTests
{
    public class TestEntity : Entity.Entity, ITestEntity
    {
        public ITestSubEntity SubEntity { get; set; }
        public int BasicValueType { get; set; }
        public string StringType { get; set; }

        public IEnumerable<string> Enumerable { get; set; }
        public IEnumerable<ITestSubEntity> MultipleSubEntities { get; set; }
    }
}
