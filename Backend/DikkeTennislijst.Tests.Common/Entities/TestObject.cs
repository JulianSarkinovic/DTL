using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Tests.Common.Entities
{
    public class TestObject : Equatable<TestObject>, ITestObject, IHasId
    {
        public TestObject()
        {
            var now = DateTimeOffset.Now;
            CreatedAt = now;
            UpdatedAt = now;
        }

        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public int SomeNumber { get; set; }
        public string? Name { get; set; }
        public string? Group { get; set; }

        public override bool IsEqual(TestObject other) => Id == other.Id;

        public override int GetHashCode() => Id;
    }
}