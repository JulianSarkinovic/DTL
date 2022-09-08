using DikkeTennisLijst.Core.Interfaces.Entities;

namespace DikkeTennisLijst.Core.Interfaces.Repositories
{
    public interface IEntityRepository<TEntity> : IObjectRepository<TEntity> where TEntity : IEntity<int>
    { }
}