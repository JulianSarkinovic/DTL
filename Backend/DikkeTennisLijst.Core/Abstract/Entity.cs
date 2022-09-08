using DikkeTennisLijst.Core.Interfaces.Entities;

namespace DikkeTennisLijst.Core.Abstract
{
    public abstract class Entity : Equatable<Entity>, IEntity<int>, ITimestamps
    {
        protected Entity()
        {
            var now = DateTimeOffset.Now;
            CreatedAt = now;
            UpdatedAt = now;
        }

        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public override bool IsEqual(Entity other) => Id == other.Id;

        public override int GetHashCode() => Id;
    }
}