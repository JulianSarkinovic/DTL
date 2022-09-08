namespace DikkeTennisLijst.Core.Interfaces.Entities
{
    public interface ITimestamps
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}