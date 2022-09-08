using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Shared.Results;

namespace DikkeTennisLijst.Core.Interfaces.Services
{
    public interface IEntityService<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Checks whether the provided entity exists, and adds the entity to the database if not so.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        Task<OperationResult<TEntity>> AddAsync(TEntity entity);

        /// <summary>
        /// Checks whether the provided entities exist, and throws if any of the entities exists.
        /// If none of the entities exist, it will add the provided entities to the database.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        Task<OperationResult<IEnumerable<TEntity>>> AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Find will return the entity with the provided ID, without children by default.
        /// The specification can be used to include children of the queried entity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="spec"></param>
        Task<OperationResult<TEntity>> GetAsync(int id, ISpecification<TEntity>? spec = null);

        /// <summary>
        /// This method will return all entities, where the spec can be used to filter and thus search.
        /// Additionally, the spec can be used to expand the query, by including (grand)children.
        /// </summary>
        /// <param name="spec">The specification for the queried entity</param>
        Task<OperationResult<List<TEntity>>> GetRangeAsync(ISpecification<TEntity>? spec = null);

        /// <summary>
        /// Updates the provided entity exactly as provided.
        /// Use this method with pre-set entities, such as usual with patch.
        /// </summary>
        /// <param name="entity"></param>
        Task<OperationResult<TEntity>> UpdatePatchAsync(TEntity entity);

        /// <summary>
        /// Fully removes the entity from the database.
        /// </summary>
        /// <param name="id">The id of the entity to remove</param>
        Task<OperationResult<TEntity>> RemoveAsync(int id);

        /// <summary>
        /// Fully removes the collection of entities from the database.
        /// </summary>
        /// <param name="entities">The entities to remove</param>
        Task<OperationResult<IEnumerable<TEntity>>> RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}