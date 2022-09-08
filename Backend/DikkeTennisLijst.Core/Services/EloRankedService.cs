using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public class EloRankedService : EloServiceBase<EloRanked>, IEloRankedService
    {
        public EloRankedService(ILogger<EloRankedService> logger, IEntityRepository<EloRanked> repository) : base(logger, repository)
        {
        }
    }
}