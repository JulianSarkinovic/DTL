using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public class SurfaceService : CoreServiceBase<Surface>, ISurfaceService
    {
        public SurfaceService(IEntityRepository<Surface> repository, ILogger<SurfaceService> logger) : base(repository, logger)
        {
        }
    }
}