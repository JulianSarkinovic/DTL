using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Results;

namespace DikkeTennisLijst.Core.Interfaces.Services
{
    public interface IPlayerService
    {
        Task<OperationResult<Player>> GetByIdAsync(string id, bool includeMatches = false);

        Task<OperationResult<Player>> GetByEmailAsync(string email);

        OperationResult<List<Player>> GetRange();

        OperationResult<List<Player>> Search(string fullName);

        Task<OperationResult<(Player Player, List<string> Roles, string Token)>> CreateAsync(Player player, string userPassword, string[] userRoles);

        Task<OperationResult<(Player Player, List<string> Roles)>> CreateStubAsync(Player player);

        Task<OperationResult<(Player Player, List<string> Roles)>> UpdateAsync(Player entity, string[] userRoles);

        Task<OperationResult> RemoveAsync(string id);
    }
}