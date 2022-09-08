using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public class EloCasualService : EloServiceBase<EloCasual>, IEloCasualService
    {
        public EloCasualService(ILogger<EloCasualService> logger, IEntityRepository<EloCasual> repository) : base(logger, repository)
        {
        }
    }
}