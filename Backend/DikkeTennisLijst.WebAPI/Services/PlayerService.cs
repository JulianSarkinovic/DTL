using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Equatables;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Services;
using DikkeTennisLijst.Core.Shared.Attributes;
using DikkeTennisLijst.Core.Shared.Enums;
using DikkeTennisLijst.Core.Shared.Helpers;
using DikkeTennisLijst.Core.Shared.Results;
using DikkeTennisLijst.Infrastructure.Configuration;
using DikkeTennisLijst.Infrastructure.Data;
using DikkeTennisLijst.WebAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DikkeTennisLijst.WebAPI.Services;

public class PlayerService : ServiceBase, IPlayerService
{
    private static List<Player> CachedPlayers { get; set; }

    private IEmailService EmailService { get; }
    private AppSettings AppSettings { get; }
    private UserManager<Player> UserManager { get; }
    private RoleManager<IdentityRole> RoleManager { get; }
    private ApplicationContext DbContext { get; }

    public PlayerService(
        IEmailService emailService,
        IObjectRepository<EmailTemplate> emailTemplateRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PlayerService> logger,
        IOptions<AppSettings> appSettings,
        UserManager<Player> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationContext context) : base(logger)
    {
        EmailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        AppSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        DbContext = context ?? throw new ArgumentNullException(nameof(context));

        CachedPlayers = UserManager.Users
            .AsNoTracking()
            .Where(x => x.Status == Status.Enabled)
            .OrderByDescending(x => x.EloRanked.Rating)
            .Include(x => x.EloRanked)
            .ToList();
    }

    public async Task<OperationResult<Player>> GetByIdAsync(string id, bool includeMatches = false)
    {
        try
        {
            var player = CachedPlayers.Find(p => p.Id.Equals(id)) ?? await UserManager.FindByIdAsync(id);
            if (player is default(Player)) throw new ArgumentOutOfRangeException(nameof(id));
            if (includeMatches)
            {
                player.Matches = DbContext.Matches
                    .AsNoTracking()
                    .Where(m => m.PlayerOneId.Equals(id) || m.PlayerTwoId.Equals(id))
                    .OrderByDescending(x => x.Duration.Start)
                    .Include(p => p.PlayerOne)
                    .Include(p => p.PlayerTwo)
                    .ToList();
            }

            return Success(player);
        }
        catch (Exception ex)
        {
            return Failure<Player>(ex);
        }
    }

    public async Task<OperationResult<Player>> GetByEmailAsync(string email)
    {
        try
        {
            var player = await UserManager.FindByIdAsync(email);
            if (player == null) throw new ArgumentOutOfRangeException(nameof(email));

            return Success(player);
        }
        catch (Exception ex)
        {
            return Failure<Player>(ex);
        }
    }

    public OperationResult<List<Player>> GetRange()
    {
        try
        {
            return Success(CachedPlayers);
        }
        catch (Exception ex)
        {
            return Failure<List<Player>>(ex);
        }
    }

    public OperationResult<List<Player>> Search(string fullName)
    {
        try
        {
            var players = fullName == null
                ? CachedPlayers
                : CachedPlayers
                    .Where(u => ($"{u.FirstName} {u.LastName}")
                    .Contains(fullName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return Success(players);
        }
        catch (Exception ex)
        {
            return Failure<List<Player>>(ex);
        }
    }

    [Todo("See comments")]
    public async Task<OperationResult<(Player Player, List<string> Roles)>> CreateStubAsync(
        Player user)
    {
        try
        {
            user.Status = Status.Stub;
            user.ConfirmationStatus = ConfirmationStatus.Awaiting;

            IdentityResult userResult = await UserManager.CreateAsync(user);

            if (!userResult.Succeeded)
            {
                _logger.LogWarning("Creating the user failed for {USER}", user);
            }

            IdentityResult rolesResult = await UserManager.AddToRoleAsync(user, Role.Player);

            if (!rolesResult.Succeeded)
            {
                _logger.LogWarning("Assigning the user to roles failed for {USER} and roles {ROLES}", user, Role.Player);
            }

            await UserManager.UpdateNormalizedEmailAsync(user);
            await UserManager.UpdateNormalizedUserNameAsync(user);
            await UserManager.UpdateSecurityStampAsync(user);

            // Todo: not like this. The person will get a match confirmation email anyways.
            // await SendConfirmationEmailAsync(user);

            var createdUser = await UserManager.FindByEmailAsync(user.Email);
            var roles = await UserManager.GetRolesAsync(createdUser);

            return Success((createdUser, roles.ToList()));
        }
        catch (Exception ex)
        {
            return Failure<(Player, List<string>)>(ex);
        }
    }

    [Todo("Look at this big boy")]
    public async Task<OperationResult<(Player Player, List<string> Roles, string Token)>> CreateAsync(
        Player player, string userPassword, string[] roles)
    {
        try
        {
            var existingUser = await UserManager.FindByNameAsync(player.UserName);
            if (existingUser != null)
            {
                if (existingUser.Status == Status.Disabled)
                {
                    // Todo: more logic must be added for a deleted user, and then subsequently recreated user.
                    _logger.LogWarning("An attempt was made to re-register a soft deleted user. Time for implementation of this scenario.");
                    return Failure<(Player, List<string>, string)>("Cannot create an account with the provided email address.");
                }

                if (existingUser.Status == Status.Stub)
                {
                    player.Id = existingUser.Id;
                    var result = await UpdateAsync(player, roles);
                    var pPlayer = result.ResultData.Player;
                    var pRoles = result.ResultData.Roles;
                    var pToken = TokenHelper.CreateSecurityToken(pPlayer.Id, pRoles, AppSettings.JwtSecretKey);

                    return Success((pPlayer, pRoles, pToken));
                }

                return Failure<(Player, List<string>, string)>("There already is an account with this email.");
            }

            var roleCheckResult = await CheckRolesAsync(player, roles);
            if (!roleCheckResult.Success)
            {
                return Failure<(Player, List<string>, string)>(roleCheckResult.ErrorMessage);
            }

            var userIdentityResult = await UserManager.CreateAsync(player, userPassword);
            if (!userIdentityResult.Succeeded)
            {
                _logger.LogWarning("Creating the user failed for {USER}", player);
                var errors = string.Join(";", userIdentityResult.Errors);
                return Failure<(Player, List<string>, string)>(errors);
            }

            var rolesIdentityResult = await UserManager.AddToRolesAsync(player, roles);
            if (!rolesIdentityResult.Succeeded)
            {
                _logger.LogWarning("Assigning the user to roles failed for {USER} and roles {ROLES}", player, roles);
                var errors = string.Join(";", userIdentityResult.Errors);
                return Failure<(Player, List<string>, string)>(errors);
            }

            await UserManager.UpdateNormalizedEmailAsync(player);
            await UserManager.UpdateNormalizedUserNameAsync(player);
            await UserManager.UpdateSecurityStampAsync(player);

            var confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(player);
            confirmationToken = confirmationToken.Replace("+", "%2B");
            await EmailService.SendAccountConfirmationEmailAsync(player, confirmationToken);

            var createdPlayer = await UserManager.FindByEmailAsync(player.Email);
            var playersRoles = await UserManager.GetRolesAsync(createdPlayer);
            var token = TokenHelper.CreateSecurityToken(createdPlayer.Id, playersRoles, AppSettings.JwtSecretKey);

            return Success((createdPlayer, playersRoles.ToList(), token));
        }
        catch (Exception ex)
        {
            return Failure<(Player, List<string>, string)>(ex);
        }
    }

    public async Task<OperationResult<(Player Player, List<string> Roles)>> UpdateAsync(Player player, string[] newRoles)
    {
        try
        {
            var existingPlayer = await UserManager.FindByIdAsync(player.Id);
            if (existingPlayer == null) throw new ArgumentOutOfRangeException(nameof(player));
            if (player.UserName != existingPlayer.UserName && UserManager.FindByNameAsync(player.UserName).Result != null)
            {
                throw new ArgumentException("Username is already in use");
            }

            var roleCheckResult = await CheckRolesAsync(player, newRoles);
            if (!roleCheckResult.Success)
            {
                return Failure<(Player, List<string>)>(roleCheckResult.ErrorMessage);
            }

            var roles = await UserManager.GetRolesAsync(existingPlayer);
            await UserManager.RemoveFromRolesAsync(existingPlayer, roles);
            await UserManager.AddToRolesAsync(existingPlayer, newRoles);

            existingPlayer.FirstName = player.FirstName;
            existingPlayer.LastName = player.LastName;
            existingPlayer.Email = player.Email;
            existingPlayer.UserName = player.UserName;
            existingPlayer.PhoneNumber = player.PhoneNumber;
            existingPlayer.Gender = player.Gender;
            existingPlayer.DateOfBirth = player.DateOfBirth;
            existingPlayer.Status = player.Status;

            await UserManager.UpdateAsync(existingPlayer);
            await UserManager.UpdateNormalizedEmailAsync(existingPlayer);
            await UserManager.UpdateNormalizedUserNameAsync(existingPlayer);
            await UserManager.UpdateSecurityStampAsync(existingPlayer);

            return Success((existingPlayer, newRoles.ToList()));
        }
        catch (Exception ex)
        {
            return Failure<(Player, List<string>)>(ex);
        }
    }

    /// <summary>
    /// This method will fully remove the user corresponding to the provided ID.
    /// </summary>
    /// <param name="id"></param>
    [Todo("Deleting user data should be thought about. Relations, comments, etc.")]
    public async Task<OperationResult> RemoveAsync(string id)
    {
        try
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null || user.Status == Status.Disabled) throw new ArgumentOutOfRangeException(nameof(id));
            await UserManager.DeleteAsync(user);
            return Success();
        }
        catch (Exception ex)
        {
            return Failure(ex);
        }
    }

    private async Task<OperationResult> CheckRolesAsync(Player player, IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            if (!await RoleManager.RoleExistsAsync(role))
            {
                _logger.LogWarning(
                    "An attempt was made to register a player into an non-existing role." +
                    "Player = {Player}", player);

                return Failure($"Adding a player as {role} is not possible.");
            }
            if (role.Contains(Role.Admin, StringComparison.OrdinalIgnoreCase)
                && !await UserManager.IsInRoleAsync(player, Role.Admin))
            {
                _logger.LogWarning(
                    "An attempt was made to register an administrator. " +
                    "Player = {Player}", player);

                return Failure($"Adding a player as {role} is not allowed.");
            }
        }

        return Success();
    }
}