using AutoMapper;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Shared.Specifications;
using DikkeTennisLijst.WebAPI.Models;
using DikkeTennisLijst.WebAPI.Models.Request;
using DikkeTennisLijst.WebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DikkeTennisLijst.WebAPI.Controllers
{
    public class MatchController : ControllerBase<Match>
    {
        private ILogger<MatchController> Logger { get; }
        private IMatchService MatchService { get; }

        public MatchController(
            ILogger<MatchController> logger,
            IMatchService matchService,
            IMapper mapper) : base(mapper)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MatchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<MatchResponseModel>>>> GetAsync()
        {
            var result = await MatchService.GetRangeAsync();
            return Result<List<Match>, List<MatchResponseModel>>(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<MatchFullResponseModel>>> GetFullAsync(int id)
        {
            var spec = new SpecificationMatchWithChildren();
            var result = await MatchService.GetAsync(id, spec);
            return Result<Match, MatchFullResponseModel>(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<MatchResponseModel>>> GetAsync(int id)
        {
            var result = await MatchService.GetAsync(id);
            return Result<Match, MatchResponseModel>(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MatchResponseModel>>> PostAsync(
            MatchRequestModel matchRequestModel)
        {
            if (matchRequestModel.PlayerOneId != UserId && matchRequestModel.PlayerTwoId != UserId)
            {
                Logger.LogWarning(
                    "The user {USER.ID} tried to post a match of which he wasn't part. " +
                    "The match request model: {@MATCHREQUESTMODEL}", UserId, matchRequestModel);

                return new ForbidResult();
            }

            matchRequestModel.Format = Match.GetFormat(matchRequestModel.Sets);
            var match = _mapper.Map<Match>(matchRequestModel);

            var result = matchRequestModel.Id == 0
                ? await MatchService.AddAsync(match, UserId)
                : await MatchService.EditAsync(match, matchRequestModel.Id, UserId);

            return Result<Match, MatchResponseModel>(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ConfirmMatchAsync(MatchConfirmationDto model)
        {
            var result = await MatchService.ConfirmMatchAsync(model.MatchId, model.Token, model.Agreed);
            return Result(result);
        }
    }
}