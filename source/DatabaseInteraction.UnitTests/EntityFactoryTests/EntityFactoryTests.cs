using System;
using System.Linq;
using DatabaseInteraction.Entity;
using NUnit.Framework;
using Telerik.JustMock;

namespace DatabaseInteraction.UnitTests.EntityFactoryTests
{
    public class EntityFactoryTests
    {
        [Test]
        public void CreateNewInstanceTest()
        {
            GivenEntityFactory();
            WhenCreateNewInstance();
            ThenCorrectValues();
        }

        private EntityFactory _entityFactory;

        private ITestEntity _mockedEntity;
        private TestEntity _created;

        private void GivenEntityFactory()
        {
            _entityFactory = new EntityFactory(new EntityMapping[]
            {
                new EntityMapping.TypedEntityMapping<ITestEntity, TestEntity>(),
                new EntityMapping.TypedEntityMapping<ITestSubEntity, TestSubEntity>(),
            });
        }

        private void WhenCreateNewInstance()
        {
            CreateMock();
            _created = _entityFactory.CreateImplementation<ITestEntity, TestEntity>(_mockedEntity);
        }

        private void ThenCorrectValues()
        {
            Assert.AreEqual(Guid.Parse("ef26fd94-1c9e-4393-8143-34dad7c2d903"), _created.Id);
            Assert.AreEqual(10, _created.BasicValueType);
            Assert.AreEqual("10", _created.StringType);
            Assert.AreEqual(new[] { "first", "second" }, _created.Enumerable);

            Assert.AreEqual(new[] { 5, 6, 7 }, _created.SubEntity.SomeMoreValues);
            Assert.AreEqual("Some", _created.SubEntity.SomeValue);

            Assert.AreEqual(new[] { 5, 6, 7 }, _created.MultipleSubEntities.First().SomeMoreValues);
            Assert.AreEqual("Some", _created.MultipleSubEntities.First().SomeValue);

            Assert.AreEqual(new[] { 5, 6, 7 }, _created.MultipleSubEntities.Skip(1).First().SomeMoreValues);
            Assert.AreEqual("Some", _created.MultipleSubEntities.Skip(1).First().SomeValue);
        }

        private void CreateMock()
        {
            var entityMock = Mock.Create<ITestEntity>();

            Mock.Arrange(() => entityMock.Id).Returns(Guid.Parse("ef26fd94-1c9e-4393-8143-34dad7c2d903"));
            Mock.Arrange(() => entityMock.BasicValueType).Returns(10);
            Mock.Arrange(() => entityMock.StringType).Returns("10");
            Mock.Arrange(() => entityMock.Enumerable).Returns(new[] { "first", "second" });

            var subEntityMock = Mock.Create<ITestSubEntity>();

            Mock.Arrange(() => subEntityMock.SomeMoreValues).Returns(new[] { 5, 6, 7 });
            Mock.Arrange(() => subEntityMock.SomeValue).Returns("Some");

            Mock.Arrange(() => entityMock.SubEntity).Returns(subEntityMock);
            Mock.Arrange(() => entityMock.MultipleSubEntities).Returns(new[] {subEntityMock, subEntityMock});

            _mockedEntity = entityMock;
        }
    }
}
