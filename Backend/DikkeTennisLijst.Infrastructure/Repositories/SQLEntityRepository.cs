using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DikkeTennisLijst.Infrastructure.Repositories
{
    public class SQLEntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public SQLEntityRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> GetAsync(ISpecification<TEntity> spec)
        {
            var entities = GetFromSpecification(spec);

            try
            {
                return await entities.SingleAsync();
            }
            catch (InvalidOperationException)
            {
                throw NonExistentElementException.New(spec);
            }
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw NonExistentElementException.New<TEntity>(id);
        }

        public async Task<TEntity> GetAsync(int id, ISpecification<TEntity> spec)
        {
            var entities = GetFromSpecification(spec);

            try
            {
                return await entities.SingleAsync(x => x.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw NonExistentElementException.New(id, spec);
            }
        }

        public IAsyncEnumerable<TEntity> GetRangeAsync(ISpecification<TEntity> spec = null)
        {
            if (spec == null) return _dbSet.AsAsyncEnumerable();
            return GetFromSpecification(spec);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (!await ExistsAsync(entity.Id))
            {
                throw NonExistentElementException.New(entity);
            }

            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (!await ExistsRangeAsync(entities))
            {
                throw new NonExistentElementException($"One or more of the {typeof(TEntity).Name}s does not exist.");
            }

            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateAsync(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
                when (ex.Message == "Attempted to update or delete an entity that does not exist in the store.")
            {
                throw NonExistentElementException.New<TEntity>(ex);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> myList)
        {
            try
            {
                _context.RemoveRange(myList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
                when (ex.Message == "Attempted to update or delete an entity that does not exist in the store.")
            {
                throw NonExistentElementException.New<TEntity>(ex);
            }
        }

        public async Task DeleteRangeAsync(ISpecification<TEntity> spec)
        {
            try
            {
                var entities = GetRangeAsync(spec).ToEnumerable();
                _context.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
                when (ex.Message == "Attempted to update or delete an entity that does not exist in the store.")
            {
                throw NonExistentElementException.New<TEntity>(ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            await DeleteAsync(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await ((IQueryable<TEntity>)_dbSet).AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (!await ExistsAsync(entity.Id))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> ExistsRangeAsync(ISpecification<TEntity> spec)
        {
            return await _dbSet.AnyAsync(spec.Criteria);
        }

        private IAsyncEnumerable<TEntity> GetFromSpecification(ISpecification<TEntity> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_context.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return secondaryResult
                            .Where(spec.Criteria)
                            .AsAsyncEnumerable();
        }
    }
}