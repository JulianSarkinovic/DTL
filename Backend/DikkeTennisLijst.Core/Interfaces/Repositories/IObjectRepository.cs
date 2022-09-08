using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Shared.Exceptions;

namespace DikkeTennisLijst.Core.Interfaces.Repositories
{
    public interface IObjectRepository<T>
    {
        /// <summary>
        /// Adds the <paramref name="obj"/> to storage.
        /// Throws a <see cref="DuplicateElementException"/> if the <typeparamref name="T"/> already exists.
        /// </summary>
        /// <exception cref="DuplicateElementException"></exception>
        Task AddAsync(T obj);

        /// <summary>
        /// Adds the <paramref name="objects"/> to storage.
        /// Throws a <see cref="DuplicateElementException"/> if any of the <typeparamref name="T"/>s already exist.
        /// </summary>
        /// <exception cref="DuplicateElementException"/>
        Task AddRangeAsync(IEnumerable<T> objects);

        /// <summary>
        /// Gets the <typeparamref name="T"/> from storage.
        /// Throws a <see cref="NonExistentElementException"/> if the <typeparamref name="T"/> does not exist.
        /// </summary>
        /// <param name="id">
        /// The identifier of the object.
        /// Likely whatever <typeparamref name="T"/><see cref="object.GetHashCode()"/> returns.
        /// </param>
        /// <exception cref="NonExistentElementException"/>
        Task<T> GetAsync(int id);

        /// <summary>
        /// Gets the <typeparamref name="T"/> from storage.
        /// Throws a <see cref="NonExistentElementException"/> if the <typeparamref name="T"/> does not exist.
        /// </summary>
        /// <param name="id">
        /// The identifier of the object.
        /// Likely whatever <typeparamref name="T"/><see cref="object.GetHashCode()"/> returns.
        /// </param>
        /// <param name="spec">
        /// The <see cref="ISpecification{T}"/> to which the <typeparamref name="T"/> is queried against.
        /// </param>
        /// <exception cref="NonExistentElementException"/>
        Task<T> GetAsync(int id, ISpecification<T> spec);

        /// <summary>
        /// Gets the <typeparamref name="T"/> from storage.
        /// Throws a <see cref="NonExistentElementException"/> if no such <typeparamref name="T"/> exists.
        /// </summary>
        /// <param name="spec">
        /// The <see cref="ISpecification{T}"/> to which the <typeparamref name="T"/> is queried against.
        /// </param>
        /// <exception cref="NonExistentElementException"/>
        Task<T> GetAsync(ISpecification<T> spec);

        /// <summary>
        /// Gets the <typeparamref name="T"/>s from storage.
        /// </summary>
        /// <param name="spec">
        /// The <see cref="ISpecification{T}"/> to which the <typeparamref name="T"/>s are queried against.
        /// </param>
        IAsyncEnumerable<T> GetRangeAsync(ISpecification<T>? spec = null);

        /// <summary>
        /// Updates the <paramref name="obj"/> in storage.
        /// Throws a <see cref="NonExistentElementException"/> if <paramref name="obj"/> does not exist.
        /// </summary>
        /// <param name="obj">The <typeparamref name="T"/> to update.</param>
        /// <exception cref="NonExistentElementException" />
        Task UpdateAsync(T obj);

        /// <summary>
        /// Updates the <paramref name="objects"/> in storage.
        /// Throws a <see cref="NonExistentElementException"/> if any of <paramref name="objects"/> does not exist.
        /// </summary>
        /// <param name="objects">The <typeparamref name="T"/>s to update.</param>
        /// <exception cref="NonExistentElementException" />
        Task UpdateRangeAsync(IEnumerable<T> objects);

        /// <summary>
        /// Updates the <paramref name="obj"/> in storage, or adds it if it does not exist.
        /// </summary>
        /// <param name="obj">The <typeparamref name="T"/> to update, or add.</param>
        Task AddOrUpdateAsync(T obj);

        /// <summary>
        /// Updates the <paramref name="objects"/> in storage, or adds them if they do not exist.
        /// </summary>
        /// <param name="objects">The <typeparamref name="T"/>s to update.</param>
        Task AddOrUpdateRangeAsync(IEnumerable<T> objects);

        /// <summary>
        /// Deletes the <typeparamref name="T"/> from storage.
        /// Throws a <see cref="NonExistentElementException"/> if the <paramref name="obj"/> does not exists.
        /// </summary>
        /// <param name="obj">The <typeparamref name="T"/> to delete.</param>
        /// <exception cref="NonExistentElementException" />
        Task DeleteAsync(T obj);

        /// <summary>
        /// Deletes the <typeparamref name="T"/> from storage.
        /// Throws a <see cref="NonExistentElementException"/> if the <typeparamref name="T"/> does not exists.
        /// </summary>
        /// <param name="id">
        /// The identifier of the <typeparamref name="T"/>.
        /// Likely whatever <typeparamref name="T"/><see cref="object.GetHashCode()"/> returns.
        /// </param>
        /// <exception cref="NonExistentElementException"/>
        Task DeleteAsync(int id);

        /// <summary>
        /// Deletes the <typeparamref name="T"/>s from storage.
        /// Throws a <see cref="NonExistentElementException"/> if any of the <paramref name="objects"/> does not exists.
        /// </summary>
        /// <param name="objects">The <typeparamref name="T"/>s to delete.</param>
        /// <exception cref="NonExistentElementException"/>
        Task DeleteRangeAsync(IEnumerable<T> objects);

        /// <summary>
        /// Deletes the <typeparamref name="T"/>s from storage.
        /// </summary>
        /// <param name="spec">
        /// The <see cref="ISpecification{T}"/> to which the <typeparamref name="T"/>s are queried against.
        /// </param>
        Task DeleteRangeAsync(ISpecification<T>? spec = null);

        /// <summary>
        /// Checks if the <typeparamref name="T"/> exists in storage.
        /// </summary>
        /// <param name="id">
        /// The identifier of the <typeparamref name="T"/>.
        /// Likely whatever <typeparamref name="T"/><see cref="object.GetHashCode()"/> returns.
        /// </param>
        /// <returns><see cref="true"/> if the <typeparamref name="T"/> exists, <see cref="false"/> otherwise.</returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Checks if any <typeparamref name="T"/>s exist in storage that comply with the <see cref="ISpecification{T}.Criteria"/> of the <paramref name="spec"/>.
        /// </summary>
        /// <param name="spec">
        /// The <see cref="ISpecification{T}"/> to which the <typeparamref name="T"/>s are queried against.
        /// </param>
        /// <returns><see cref="true"/> if any <typeparamref name="T"/> exists, <see cref="false"/> otherwise.</returns>
        Task<bool> ExistsRangeAsync(ISpecification<T> spec);

        /// <summary>
        /// Checks if all the <paramref name="objects"/> exist in storage.
        /// </summary>
        /// <param name="objects">The <typeparamref name="T"/>s to check existence for.</param>
        /// <returns><see cref="true"/> if all <paramref name="objects"/> exists, <see cref="false"/> otherwise.</returns>
        Task<bool> ExistsRangeAsync(IEnumerable<T> objects);
    }
}