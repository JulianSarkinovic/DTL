using DikkeTennisLijst.Core.Shared.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DikkeTennisLijst.Core.Services
{
    public abstract class ServiceBase
    {
        protected readonly ILogger<ServiceBase> _logger;

        protected ServiceBase(ILogger<ServiceBase> logger)
        {
            _logger = logger;
        }

        #region OperationResult<T>

        protected static OperationResult<T> Success<T>(T data, string? route = null)
        {
            return OperationResult<T>.CreateSuccess(data, route);
        }

        protected static OperationResult<T> Failure<T>(string? errorMessage = null, string? route = null)
        {
            return OperationResult<T>.CreateFailure(errorMessage, route);
        }

        protected OperationResult<T> Failure<T>(Exception ex, string? customErrorMessage = null, string? route = null)
        {
            _logger.LogError(ex, ex.Message);
            return OperationResult<T>.CreateFailure(ex, customErrorMessage, route);
        }

        #endregion OperationResult<T>

        #region OperationResult

        protected static OperationResult Success(string? route = null)
        {
            return OperationResult.CreateSuccess(route);
        }

        protected static OperationResult Failure(string? errorMessage = null, string? route = null)
        {
            return OperationResult.CreateFailure(errorMessage, route);
        }

        protected OperationResult Failure(Exception ex, string? customErrorMessage = null, string? route = null)
        {
            _logger.LogError(ex, customErrorMessage ?? ex.Message);
            return OperationResult.CreateFailure(ex, customErrorMessage, route);
        }

        protected OperationResult Failure(IdentityResult identityResult, string? route = null)
        {
            string message = "";
            foreach (var error in identityResult.Errors)
            {
                message += $"{error.Description} ";
            }

            _logger.LogWarning("The message returned from the identity result where {DESCRIPTIONS}", message);
            return OperationResult.CreateFailure(message, route);
        }

        #endregion OperationResult
    }
}