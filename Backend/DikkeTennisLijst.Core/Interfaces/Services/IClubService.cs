using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Results;

namespace DikkeTennisLijst.Core.Interfaces.Services
{
    public interface IClubService : IEntityService<Club>
    {
        Task<OperationResult<Club>> UpdateAsync(Club entity, int id);
    }
}