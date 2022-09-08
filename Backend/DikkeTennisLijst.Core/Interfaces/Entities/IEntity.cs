namespace DikkeTennisLijst.Core.Interfaces.Entities
{
    public interface IEntity<T> where T : IComparable
    {
        T Id { get; set; }
    }
}