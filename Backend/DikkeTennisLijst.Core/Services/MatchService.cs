using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Calculations;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Shared.Attributes;
using DikkeTennisLijst.Core.Shared.Enums;
using DikkeTennisLijst.Core.Shared.Results;
using DikkeTennisLijst.Core.Shared.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public class MatchService : CoreServiceBase<Match>, IMatchService
    {
        private IEmailService EmailService { get; }
        private IEntityRepository<EloCasual> EloCasualRepository { get; }
        private IEntityRepository<EloRanked> EloRankedRepository { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<Player> UserManager { get; }

        public MatchService(
            IEntityRepository<Match> repository,
            IEntityRepository<EloCasual> eloCasualRepository,
            IEntityRepository<EloRanked> eloRankedRepository,
            ILogger<MatchService> logger,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            UserManager<Player> userManager) : base(repository, logger)
        {
            EmailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            EloCasualRepository = eloCasualRepository ?? throw new ArgumentNullException(nameof(eloCasualRepository));
            EloRankedRepository = eloRankedRepository ?? throw new ArgumentNullException(nameof(eloRankedRepository));
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<OperationResult<Match>> AddAsync(Match match, string userId)
        {
            try
            {
                await Repository.AddAsync(match);
                await SendConfirmationEmailAsync(match, userId, isEdit: false);

                return Success(match);
            }
            catch (Exception ex)
            {
                return Failure<Match>(ex);
            }
        }

        public async Task<OperationResult<Match>> EditAsync(Match match, int matchId, string userId)
        {
            try
            {
                var existingMatch = await Repository.GetAsync(matchId) ?? throw new ArgumentOutOfRangeException(nameof(matchId));

                existingMatch.Update(match, newConfirmationToken: true);
                await Repository.UpdateAsync(existingMatch);
                await SendConfirmationEmailAsync(match, userId, isEdit: true);

                return Success(match);
            }
            catch (Exception ex)
            {
                return Failure<Match>(ex);
            }
        }

        public async Task<OperationResult<Match>> UpdateAsync(Match match, int id)
        {
            try
            {
                var existingMatch = await Repository.GetAsync(id);

                existingMatch.Update(match, newConfirmationToken: false);
                await Repository.UpdateAsync(existingMatch);

                return Success(existingMatch);
            }
            catch (Exception ex)
            {
                return Failure<Match>(ex);
            }
        }

        private async Task SendConfirmationEmailAsync(Match match, string userId, bool isEdit)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(userId);

                var opponentId = userId.Equals(match.PlayerOneId) ? match.PlayerTwoId : match.PlayerOneId;
                var opponent = await UserManager.FindByIdAsync(opponentId);

                await EmailService.SendMatchConfirmationEmailAsync(user, opponent, match, isEdit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [Todo("This should update match and elo's as a single transaction.")]
        public async Task<OperationResult> ConfirmMatchAsync(int matchId, string token, bool agreed)
        {
            try
            {
                var match = await Repository.GetAsync(matchId);

                if (match is null)
                {
                    return Failure(route: "match-not-found");
                }
                else if (match.ConfirmationStatus == ConfirmationStatus.Confirmed)
                {
                    return Failure(route: "already-confirmed");
                }
                else if (match.ConfirmationStatus == ConfirmationStatus.Declined)
                {
                    return Failure(route: "already-declined");
                }
                else if (!match.ConfirmationToken.Equals(token, StringComparison.OrdinalIgnoreCase))
                {
                    return Failure(route: "token-not-matched");
                }

                match.ConfirmationStatus = agreed ? ConfirmationStatus.Confirmed : ConfirmationStatus.Declined;
                match.Status = Status.Enabled;
                await Repository.UpdateAsync(match);

                async Task UpdateElosAsync<TElo>(IEntityRepository<TElo> eloRepository, Match match) where TElo : Elo
                {
                    // 1 Get elo;
                    var eloP1 = await eloRepository.GetAsync(new SpecificationEntity<TElo>(x => x.PlayerId == match.PlayerOneId));
                    var eloP2 = await eloRepository.GetAsync(new SpecificationEntity<TElo>(x => x.PlayerId == match.PlayerTwoId));

                    // 2 Update elo for match;
                    EloCalculator.UpdateElosForMatch(eloP1, eloP2, match);

                    // 3 Update database;
                    await eloRepository.UpdateAsync(eloP1);
                    await eloRepository.UpdateAsync(eloP2);
                }

                await UpdateElosAsync(EloCasualRepository, match);
                await UpdateElosAsync(EloRankedRepository, match);

                return Success(route: agreed ? "confirmed" : "declined");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Failure(route: "failed");
            }
        }
    }
}