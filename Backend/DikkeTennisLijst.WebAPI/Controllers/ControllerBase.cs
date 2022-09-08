using AutoMapper;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Results;
using DikkeTennisLijst.Infrastructure.Configuration;
using DikkeTennisLijst.WebAPI.Models;
using DikkeTennisLijst.WebAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DikkeTennisLijst.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ControllerBase<T> : ControllerBase
    {
        protected IMapper _mapper { get; }

        protected ControllerBase(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Whether the user that sent the request is a <see cref="Role.Player"/>.
        /// </summary>
        protected bool UserIsPlayer => User.Claims.Any(c =>
               c.Value.Equals(Role.Player, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Whether the user that sent the request is an <see cref="Role.Admin"/>.
        /// </summary>
        protected bool UserIsAdmin => User.Claims.Any(c =>
               c.Value.Equals(Role.Admin, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// The ID of the user that sent the request.
        /// </summary>
        protected string UserId => User.Identity.Name;

        /// <summary>
        /// Whether the user that sent the request is the player, or is an <see cref="Role.Admin"/>
        /// </summary>
        protected bool UserIsPlayerOrAdmin(string playerId)
        {
            return UserId.Equals(playerId) || UserIsAdmin;
        }

        /// <summary>
        /// The result to return when data should be returned.
        /// </summary>
        /// <param name="result">The operation result from the service.</param>
        /// <param name="customUserErrorMessage">An error message to override the error message provided from the service.</param>
        /// <param name="noContent">Whether no content should be returned on success.</param>
        /// <returns>
        /// 200 if the operation was a success (includes the result data).<br/>
        /// 204 if the operation was a success and <paramref name="noContent"/> was set to true.<br/>
        /// 200 if the operation was a failure (includes the error message).<br/>
        /// 404 if the operation was a failure and its exception was an <see cref="ArgumentOutOfRangeException"/>.<br/>
        /// </returns>
        protected ActionResult<ApiResponse> Result(
            OperationResult result,
            string customUserErrorMessage = null,
            bool noContent = false)
        {
            if (result.Exception is ArgumentOutOfRangeException) return NotFound();
            if (result.Success && noContent) return NoContent();

            var response = ApiResponse.CreateResponse(result, customUserErrorMessage);
            return Ok(response);
        }

        /// <summary>
        /// The result to return when data should be returned.
        /// </summary>
        /// <typeparam name="TResult">The type of the result data to be returned.</typeparam>
        /// <param name="result">The operation result from the service.</param>
        /// <param name="customUserErrorMessage">An error message to override the error message provided from the service.</param>
        /// <param name="noContent">Whether no content should be returned on success.</param>
        /// <returns>
        /// 200 if the operation was a success (includes the result data).<br/>
        /// 204 if the operation was a success and <paramref name="noContent"/> was set to true.<br/>
        /// 200 if the operation was a failure (includes the error message).<br/>
        /// 404 if the operation was a failure and its exception was an <see cref="ArgumentOutOfRangeException"/>.<br/>
        /// </returns>
        protected ActionResult<ApiResponse<TResult>> Result<TResult>(
            OperationResult<TResult> result,
            string customUserErrorMessage = null,
            bool noContent = false) where TResult : class
        {
            if (result.Exception is ArgumentOutOfRangeException) return NotFound();
            if (result.Success && noContent) return NoContent();

            var response = ApiResponse<TResult>.CreateResponse(result, customUserErrorMessage);
            return Ok(response);
        }

        /// <summary>
        /// The result to return when data from should be returned after mapping it first.
        /// </summary>
        /// <typeparam name="TInput">The type of the result data received from the service.</typeparam>
        /// <typeparam name="TResult">The type of the result data to be returned.</typeparam>
        /// <param name="result">The operation result from the service.</param>
        /// <param name="customUserErrorMessage">An error message to override the error message provided from the service.</param>
        /// <param name="noContent">Whether no content should be returned on success.</param>
        /// <returns>
        /// 200 if the operation was a success (includes the result data).<br/>
        /// 204 if the operation was a success and <paramref name="noContent"/> was set to true.<br/>
        /// 200 if the operation was a failure (includes the error message).<br/>
        /// 404 if the operation was a failure and its exception was an <see cref="ArgumentOutOfRangeException"/>.<br/>
        /// </returns>
        protected ActionResult<ApiResponse<TResult>> Result<TInput, TResult>(
            OperationResult<TInput> result,
            string customUserErrorMessage = null,
            bool noContent = false) where TInput : class where TResult : class
        {
            if (result.Exception is ArgumentOutOfRangeException) return NotFound();
            if (result.Success && noContent) return NoContent();
            if (result.Success && result.ResultData != null)
            {
                var mappedResult = _mapper.Map<TResult>(result.ResultData);
                var mappedResponse = ApiResponse<TResult>.CreateSuccess(mappedResult);
                return Ok(mappedResponse);
            }

            var response = ApiResponse<TResult>.CreateResponse(result, customUserErrorMessage);
            return Ok(response);
        }

        /// <summary>
        /// The result to return when data from should be returned after mapping it first.
        /// </summary>
        /// <param name="result">The operation result from the service.</param>
        /// <param name="customUserErrorMessage">An error message to override the error message provided from the service.</param>
        /// <returns>
        /// 200 if the operation was a success (includes the result data).<br/>
        /// 200 if the operation was a failure (includes the error message).<br/>
        /// 404 if the operation was a failure and its exception was an <see cref="ArgumentOutOfRangeException"/>.<br/>
        /// </returns>
        protected ActionResult<ApiResponse<PlayerResponseModel>> Result(
            OperationResult<(Player Player, List<string> Roles)> result,
            string customUserErrorMessage = null)
        {
            if (result.Exception is ArgumentOutOfRangeException) return NotFound();
            if (result.Success
                    && result.ResultData.Player != null
                    && result.ResultData.Roles != null)
            {
                var mappedResult = _mapper.Map<PlayerResponseModel>(result.ResultData.Player);
                mappedResult.Roles = result.ResultData.Roles.ToArray();
                var mappedResponse = ApiResponse<PlayerResponseModel>.CreateSuccess(mappedResult);
                return Ok(mappedResponse);
            }

            var failedResult = OperationResult<PlayerResponseModel>.CreateFailure(
                result.Exception, result.ErrorMessage, result.Route);

            var response = ApiResponse<PlayerResponseModel>.CreateResponse(failedResult, customUserErrorMessage);
            return Ok(response);
        }

        /// <summary>
        /// The result to return when data from should be returned after mapping it first.
        /// </summary>
        /// <param name="result">The operation result from the service.</param>
        /// <param name="customUserErrorMessage">An error message to override the error message provided from the service.</param>
        /// <returns>
        /// 200 if the operation was a success (includes the result data).<br/>
        /// 200 if the operation was a failure (includes the error message).<br/>
        /// 404 if the operation was a failure and its exception was an <see cref="ArgumentOutOfRangeException"/>.<br/>
        /// </returns>
        protected ActionResult<ApiResponse<PlayerResponseModel>> Result(
            OperationResult<(Player Player, List<string> Roles, string Token)> result,
            string customUserErrorMessage = null)
        {
            if (result.Exception is ArgumentOutOfRangeException) return NotFound();
            if (result.Success
                    && result.ResultData.Player != null
                    && result.ResultData.Roles != null
                    && result.ResultData.Token != null)
            {
                var mappedResult = _mapper.Map<PlayerResponseModel>(result.ResultData.Player);
                mappedResult.Roles = result.ResultData.Roles.ToArray();
                mappedResult.Token = result.ResultData.Token;

                var mappedResponse = ApiResponse<PlayerResponseModel>.CreateSuccess(mappedResult);
                return Ok(mappedResponse);
            }

            var failedResult = OperationResult<PlayerResponseModel>.CreateFailure(
                result.Exception, result.ErrorMessage, result.Route);

            var response = ApiResponse<PlayerResponseModel>.CreateResponse(failedResult, customUserErrorMessage);
            return Ok(response);
        }
    }
}