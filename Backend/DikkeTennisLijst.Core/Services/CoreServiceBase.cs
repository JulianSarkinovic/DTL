using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Shared.Results;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public abstract class CoreServiceBase<TEntity> : ServiceBase, IEntityService<TEntity> where TEntity : Entity
    {
        protected readonly IEntityRepository<TEntity> Repository;

        protected CoreServiceBase(IEntityRepository<TEntity> repository, ILogger<CoreServiceBase<TEntity>> logger) : base(logger)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Checks whether the provided entity exists, and adds the entity to the database if it does not.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public virtual async Task<OperationResult<TEntity>> AddAsync(TEntity entity)
        {
            try
            {
                await Repository.AddAsync(entity);
                return Success(entity);
            }
            catch (Exception ex)
            {
                return Failure<TEntity>(ex);
            }
        }

        /// <summary>
        /// Checks whether the provided entities exist, and throws if any of the entities exists.
        /// If none of the entities exist, it will add the provided entities to the database.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        public virtual async Task<OperationResult<IEnumerable<TEntity>>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await Repository.AddRangeAsync(entities);
                return Success(entities);
            }
            catch (Exception ex)
            {
                return Failure<IEnumerable<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Find will return the entity with the provided ID, without children by default.
        /// The specification can be used to include children of the queried entity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="spec"></param>
        public virtual async Task<OperationResult<TEntity>> GetAsync(int id, ISpecification<TEntity>? spec = null)
        {
            try
            {
                var entity = spec is null ? await Repository.GetAsync(id) : await Repository.GetAsync(id, spec);
                return Success(entity);
            }
            catch (Exception ex)
            {
                return Failure<TEntity>(ex);
            }
        }

        /// <summary>
        /// This method will return all entities, where the spec can be used to filter and thus search.
        /// Additionally, the spec can be used to expand the query, by including (grand)children.
        /// </summary>
        /// <param name="spec">The specification for the queried entity</param>
        public virtual async Task<OperationResult<List<TEntity>>> GetRangeAsync(ISpecification<TEntity>? spec = null)
        {
            try
            {
                var entities = await Repository.GetRangeAsync(spec).ToListAsync();
                return Success(entities);
            }
            catch (Exception ex)
            {
                return Failure<List<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Updates the provided entity exactly as provided.
        /// Use this method with pre-set entities, such as usual with patch.
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task<OperationResult<TEntity>> UpdatePatchAsync(TEntity entity)
        {
            try
            {
                await Repository.UpdateAsync(entity);
                return Success(entity);
            }
            catch (Exception ex)
            {
                return Failure<TEntity>(ex);
            }
        }

        /// <summary>
        /// Fully removes the entity from the database.
        /// </summary>
        /// <param name="id">The id of the entity to remove</param>
        public virtual async Task<OperationResult<TEntity>> RemoveAsync(int id)
        {
            try
            {
                var existingEntity = await Repository.GetAsync(id);
                await Repository.DeleteAsync(existingEntity);
                return Success(existingEntity);
            }
            catch (Exception ex)
            {
                return Failure<TEntity>(ex);
            }
        }

        /// <summary>
        /// Fully removes the collection of entities from the database.
        /// </summary>
        /// <param name="entities">The entities to remove</param>
        public virtual async Task<OperationResult<IEnumerable<TEntity>>> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await Repository.DeleteRangeAsync(entities);
                return Success(entities);
            }
            catch (Exception ex)
            {
                return Failure<IEnumerable<TEntity>>(ex);
            }
        }
    }
}