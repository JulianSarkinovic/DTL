using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Shared.Exceptions;
using ObjectSerializer = DikkeTennisLijst.Infrastructure.Serialization.ObjectSerializer;

namespace DikkeTennisLijst.Infrastructure.Repositories
{
    public class BlobObjectRepository<T> : IObjectRepository<T>
    {
        private BlobContainerClient ContainerClient { get; }
        private string BaseFolder { get; } = typeof(T).Name;
        private Dictionary<Type, int> EntityHighestIds { get; } = new();

        public BlobObjectRepository(BlobServiceClient blobServiceClient)
        {
            ContainerClient = blobServiceClient.GetBlobContainerClient("files".ToLower());
            ContainerClient.CreateIfNotExists();
        }

        #region IRepository methods

        public async Task AddAsync(T obj)
        {
            try
            {
                if (obj is IEntity<int> entity)
                {
                    if (entity.Id != 0)
                    {
                        throw new ArgumentException($"Should not add an {typeof(T).Name} with the {nameof(entity.Id)} property set.", nameof(obj));
                    }

                    if (EntityHighestIds.TryGetValue(typeof(T), out int id))
                    {
                        id++;
                        entity.Id = id;
                        EntityHighestIds[typeof(T)] = id;
                    }
                    else
                    {
                        var blobs = ListBlobsAsync();
                        var maxId = await blobs.AnyAsync()
                            ? await blobs.MaxAsync(x => GetIdentifier(x.Name))
                            : 0;

                        maxId++;
                        entity.Id = maxId;
                        EntityHighestIds[typeof(T)] = maxId;
                    }
                }

                await UploadAsync(obj, overwrite: false);
            }
            catch (Azure.RequestFailedException ex) when (ex.ErrorCode.Equals("BlobAlreadyExists"))
            {
                throw DuplicateElementException.New(obj, ex);
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> objects)
        {
            foreach (var obj in objects)
            {
                await AddAsync(obj);
            }
        }

        public async Task<T> GetAsync(ISpecification<T> spec)
        {
            try
            {
                return await GetRangeAsync(spec).SingleAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new NonExistentElementException($"The {typeof(T).Name} sought does not exist.", ex);
            }
        }

        public async Task<T> GetAsync(int id)
        {
            try
            {
                var blobClient = GetBlobClient(id);
                using var stream = await blobClient.OpenReadAsync();
                return await ObjectSerializer.DeserializeAsync<T>(stream);
            }
            catch (Azure.RequestFailedException ex) when (ex.ErrorCode.Equals("BlobNotFound"))
            {
                throw NonExistentElementException.New<T>(ex);
            }
        }

        public async Task<T> GetAsync(int id, ISpecification<T> spec)
        {
            try
            {
                var entity = await GetAsync(id);
                var matcher = spec.Criteria.Compile();
                var matched = matcher(entity);
                return matched
                    ? entity
                    : throw new NonExistentElementException(
                        $"The ID {id} matches an {typeof(T).Name} but the {nameof(ISpecification<T>)} does not.");
            }
            catch (Exception ex) when
                ((ex is Azure.RequestFailedException rfex && rfex.ErrorCode.Equals("BlobNotFound"))
                  || ex is NonExistentElementException)
            {
                throw NonExistentElementException.New<T>(ex);
            }
        }

        public async IAsyncEnumerable<T> GetRangeAsync(ISpecification<T> spec = null)
        {
            var isSought = spec?.Criteria?.Compile();

            await foreach (var blob in ListBlobsAsync())
            {
                var blobClient = GetBlobClient(blob.Name);
                using var stream = await blobClient.OpenReadAsync();
                var obj = await ObjectSerializer.DeserializeAsync<T>(stream);

                if (isSought == null || isSought(obj))
                {
                    yield return obj;
                }
            }
        }

        public async Task UpdateAsync(T obj)
        {
            if (!await ExistsAsync(obj.GetHashCode()))
            {
                throw NonExistentElementException.New(obj);
            }

            await UploadAsync(obj, overwrite: true);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> objs)
        {
            if (!await ExistsRangeAsync(objs))
            {
                throw new NonExistentElementException($"One or more of the {typeof(T).Name}s does not exist.");
            }

            foreach (var obj in objs)
            {
                await UploadAsync(obj, overwrite: true);
            }
        }

        public async Task AddOrUpdateAsync(T obj)
        {
            if (await ExistsAsync(obj.GetHashCode()))
            {
                await UpdateAsync(obj);
            }
            else
            {
                await AddAsync(obj);
            }
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
            {
                await AddOrUpdateAsync(obj);
            }
        }

        public async Task DeleteAsync(T obj)
        {
            await DeleteAsync(obj.GetHashCode());
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var blobClient = GetBlobClient(id);
                await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
            }
            catch (Azure.RequestFailedException ex) when (ex.ErrorCode.Equals("BlobNotFound"))
            {
                throw NonExistentElementException.New<T>(ex);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<T> ts)
        {
            foreach (var item in ts)
            {
                await DeleteAsync(item);
            }
        }

        public async Task DeleteRangeAsync(ISpecification<T> spec = null)
        {
            await foreach (var obj in GetRangeAsync(spec))
            {
                await DeleteAsync(obj);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await ExistsRangeAsync(new[] { id });
        }

        public async Task<bool> ExistsRangeAsync(ISpecification<T> spec)
        {
            return await GetRangeAsync(spec).AnyAsync();
        }

        public async Task<bool> ExistsRangeAsync(IEnumerable<T> objects)
        {
            return await ExistsRangeAsync(objects.Select(x => x.GetHashCode()));
        }

        private async Task<bool> ExistsRangeAsync(IEnumerable<int> ids)
        {
            var existingIds = await ListBlobsAsync().Select(x => GetIdentifier(x.Name)).ToListAsync();
            return ids.All(id => existingIds.Contains(id));
        }

        #endregion IRepository methods

        #region Private helper methods

        private async Task UploadAsync(T obj, bool overwrite)
        {
            var blobClient = GetBlobClient(obj);
            using var stream = new MemoryStream();
            await ObjectSerializer.SerializeAsync(stream, obj);
            await blobClient.UploadAsync(stream, overwrite);
        }

        private async IAsyncEnumerable<BlobItem> ListBlobsAsync()
        {
            await foreach (var blobItem in ContainerClient.GetBlobsAsync(prefix: BaseFolder))
            {
                yield return blobItem;
            }
        }

        private BlobClient GetBlobClient(T obj)
        {
            return GetBlobClient(obj.GetHashCode());
        }

        private BlobClient GetBlobClient(int identifier)
        {
            var blobName = GetBlobName(identifier);
            return GetBlobClient(blobName);
        }

        private BlobClient GetBlobClient(string blobName)
        {
            return ContainerClient.GetBlobClient(blobName);
        }

        private string GetBlobName(int identifier)
        {
            return $"{BaseFolder}/{identifier}";
        }

        private int GetIdentifier(string blobName)
        {
            return int.Parse(blobName.Split('/').Last());
        }

        #endregion Private helper methods
    }
}