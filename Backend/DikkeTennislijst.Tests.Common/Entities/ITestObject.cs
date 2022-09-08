namespace DikkeTennisLijst.Tests.Common.Entities
{
    public interface ITestObject
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
        int SomeNumber { get; set; }
        string? Name { get; set; }
        string? Group { get; set; }
    }
}