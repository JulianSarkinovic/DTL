using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Tests.Common.Entities
{
    public class TestEntity : Entity, ITestObject, IHasId
    {
        public int SomeNumber { get; set; }
        public string? Name { get; set; }
        public string? Group { get; set; }
    }
}