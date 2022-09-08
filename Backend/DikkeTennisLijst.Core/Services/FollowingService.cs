using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public class FollowingService : CoreServiceBase<Following>, IFollowingService
    {
        public FollowingService(IEntityRepository<Following> repository, ILogger<FollowingService> logger) : base(repository, logger)
        {
        }
    }
}