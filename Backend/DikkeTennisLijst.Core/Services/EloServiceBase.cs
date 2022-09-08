using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public class EloServiceBase<TElo> : CoreServiceBase<TElo>, IEloService<TElo> where TElo : Elo
    {
        public EloServiceBase(ILogger<CoreServiceBase<TElo>> logger, IEntityRepository<TElo> repository) : base(repository, logger)
        {
        }
    }
}