using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Shared.Results;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public class ClubService : CoreServiceBase<Club>, IClubService
    {
        public ClubService(IEntityRepository<Club> repository, ILogger<ClubService> logger) : base(repository, logger)
        {
        }

        public async Task<OperationResult<Club>> UpdateAsync(Club entity, int id)
        {
            try
            {
                var existingEntity = await Repository.GetAsync(id);

                // Todo: see if anything else to update
                existingEntity.Name = entity.Name;
                existingEntity.RegistrationNumber = entity.RegistrationNumber;

                await Repository.UpdateAsync(existingEntity);
                return Success(existingEntity);
            }
            catch (Exception ex)
            {
                return Failure<Club>(ex);
            }
        }
    }
}