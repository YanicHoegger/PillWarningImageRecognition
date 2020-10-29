using System.Collections.Generic;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.UnitTests.EntityFactoryTests
{
    public interface ITestEntity : IEntity
    {
        public ITestSubEntity SubEntity { get; set; }
        public int BasicValueType { get; set; }
        public string StringType { get; set; }

        public IEnumerable<string> Enumerable { get; set; }
        public IEnumerable<ITestSubEntity> MultipleSubEntities { get; set; }
    }
}
