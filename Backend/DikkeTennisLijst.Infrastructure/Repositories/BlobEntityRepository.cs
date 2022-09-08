using Azure.Storage.Blobs;
using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;

namespace DikkeTennisLijst.Infrastructure.Repositories
{
    /// <summary>
    /// Restricts the <see cref="BlobObjectRepository{T}"/> so that it can only be used with
    /// <see cref="IEntity{T}"/> of <see cref="int"/>s.
    /// </summary>
    public class BlobEntityRepository<TEntity>
        : BlobObjectRepository<TEntity>, IEntityRepository<TEntity> where TEntity : IEntity<int>
    {
        public BlobEntityRepository(BlobServiceClient blobServiceClient) : base(blobServiceClient)
        {
        }
    }
}