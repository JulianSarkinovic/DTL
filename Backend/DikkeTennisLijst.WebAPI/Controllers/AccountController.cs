using AutoMapper;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Infrastructure.Configuration;
using DikkeTennisLijst.WebAPI.Models;
using DikkeTennisLijst.WebAPI.Models.Request;
using DikkeTennisLijst.WebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DikkeTennisLijst.WebAPI.Controllers
{
    public class AccountController : ControllerBase<Player>
    {
        private IAccountService AccountService { get; }
        private IPlayerService PlayerService { get; }

        public AccountController(
            IMapper mapper,
            IAccountService accountService,
            IPlayerService playerService) : base(mapper)
        {
            AccountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            PlayerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PlayerResponseModel>>> Register(
            PlayerRegisterRequestModel model)
        {
            var player = new Player(model.FirstName, model.LastName, model.Email)
            {
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber
            };

            var result = await PlayerService.CreateAsync(player, model.Password, new[] { Role.Player });
            return Result(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> RegisterConfirm(
            PlayerEmailConfirmationModel model)
        {
            var result = await AccountService.ConfirmRegistrationAsync(model.UserId, model.Token);
            return Result(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PlayerResponseModel>>> Authenticate(
            PlayerLoginRequestModel model)
        {
            var result = await AccountService.AuthenticateAsync(model.Email, model.Password);
            return Result(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> ChangePassword(
            PlayerChangePasswordRequestModel model)
        {
            var result = await AccountService.ChangePasswordAsync(model.Email, model.NewPassword, model.CurrentPassword);
            return Result(result, noContent: true);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ResetPassword(
            WrappedEmail wrappedEmail)
        {
            var result = await AccountService.ResetPasswordAsync(wrappedEmail.Email);
            return Result(result, noContent: true);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PlayerResponseModel>>> ResetPasswordConfirm(
            PlayerResetPasswordRequestModel model)
        {
            var result = await AccountService.ResetPasswordConfirmAsync(model.Email, model.NewPassword, model.Token);
            return Result(result);
        }

        public class WrappedEmail
        {
            public string Email { get; set; }
        }
    }
}