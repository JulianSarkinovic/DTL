using AutoMapper;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.WebAPI.Models;
using DikkeTennisLijst.WebAPI.Models.Request;
using DikkeTennisLijst.WebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DikkeTennisLijst.WebAPI.Controllers
{
    public class PlayerController : ControllerBase<Player>
    {
        private ILogger<PlayerController> Logger { get; }
        private IPlayerService PlayerService { get; }
        private IFollowingService FollowingService { get; }

        public PlayerController(
            IMapper mapper,
            ILogger<PlayerController> logger,
            IPlayerService userService,
            IFollowingService followingService) : base(mapper)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            PlayerService = userService ?? throw new ArgumentNullException(nameof(userService));
            FollowingService = followingService ?? throw new ArgumentNullException(nameof(followingService));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ApiResponse<List<PlayerResponseModel>>> Get()
        {
            var result = PlayerService.GetRange();
            return Result<List<Player>, List<PlayerResponseModel>>(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PlayerResponseModel>>> GetAsync(
            string id)
        {
            var result = await PlayerService.GetByIdAsync(id);
            return Result<Player, PlayerResponseModel>(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PlayerForProfileResponseModel>>> GetForProfileAsync(
            string id)
        {
            var result = await PlayerService.GetByIdAsync(id, includeMatches: true);
            return Result<Player, PlayerForProfileResponseModel>(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ApiResponse<List<PlayerResponseModel>>> Search(
            string fullName)
        {
            var result = PlayerService.Search(fullName);
            return Result<List<Player>, List<PlayerResponseModel>>(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PlayerResponseModel>>> FindOrCreateAsync(
            PlayerCreationRequestModel model)
        {
            var findResult = await PlayerService.GetByEmailAsync(model.Email);
            if (findResult != null) return Result<Player, PlayerResponseModel>(findResult);

            var opponent = new Player(model.FirstName, model.LastName, model.Email);
            var createResult = await PlayerService.CreateStubAsync(opponent);

            await TryCreateFollowing(createResult.ResultData.Player.Id);

            return Result(createResult);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Put(
            string id, PlayerRequestModel model)
        {
            if (id != model.Id) return BadRequest();
            if (!UserIsPlayerOrAdmin(id)) return Unauthorized();

            var user = _mapper.Map<Player>(model);
            user.Id = model.Id;
            var result = await PlayerService.UpdateAsync(user, model.Roles);

            return Result(result, noContent: true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteAsync(string id)
        {
            if (!UserIsPlayerOrAdmin(id)) return Unauthorized();
            var result = await PlayerService.RemoveAsync(id);

            return Result(result, noContent: true);
        }

        private async Task TryCreateFollowing(string opponentId)
        {
            try
            {
                var following = new Following(UserId, opponentId);
                var followingResult = await FollowingService.AddAsync(following);
                if (!followingResult.Success)
                {
                    Logger.LogWarning("Creating a following was not successful {FOLLOWING}", following);
                }
            }
            catch (Exception)
            {
                Logger.LogWarning($"Creating a following was not successful for players {UserId} and {opponentId}");
            }
        }
    }
}