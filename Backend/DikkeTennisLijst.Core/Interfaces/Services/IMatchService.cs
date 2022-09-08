using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Results;

namespace DikkeTennisLijst.Core.Interfaces.Services
{
    public interface IMatchService : IEntityService<Match>
    {
        Task<OperationResult<Match>> AddAsync(Match match, string userId);

        Task<OperationResult<Match>> EditAsync(Match match, int matchId, string userId);

        Task<OperationResult> ConfirmMatchAsync(int matchId, string token, bool agreed);

        Task<OperationResult<Match>> UpdateAsync(Match match, int id);
    }
}