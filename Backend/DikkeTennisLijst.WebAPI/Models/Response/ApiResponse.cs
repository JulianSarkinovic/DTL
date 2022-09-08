using DikkeTennisLijst.Core.Shared.Results;

namespace DikkeTennisLijst.WebAPI.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Route { get; set; }

        protected ApiResponse()
        { }

        public static ApiResponse CreateFailure(string errorMessage, string route = null) => new()
        {
            Success = false,
            ErrorMessage = errorMessage,
            Route = route
        };

        public static ApiResponse CreateSuccess(string route = null) => new()
        {
            Success = true,
            Route = route
        };

        /// <summary>
        /// The API Response to use if it can be directly mapped from the OperationResult.
        /// </summary>
        public static ApiResponse CreateResponse(
            OperationResult operationResult,
            string customErrorMessage = null) => new()
            {
                Success = operationResult.Success,
                ErrorMessage = operationResult.Exception != null
                    ? "Something went wrong on the server."
                    : customErrorMessage ?? operationResult.ErrorMessage,
                Route = operationResult.Route,
            };
    }

    public sealed class ApiResponse<TResult> : ApiResponse where TResult : class
    {
        public TResult ResultData { get; init; }

        private ApiResponse()
        { }

        /// <summary>
        /// The API Response to use if it can be directly mapped from the OperationResult.
        /// </summary>
        public static ApiResponse<TResult> CreateResponse(
            OperationResult<TResult> operationResult,
            string customErrorMessage = null) => new()
            {
                Success = operationResult.Success,
                ResultData = operationResult.ResultData,
                ErrorMessage = operationResult.Exception != null
                ? "Something went wrong on the server."
                : customErrorMessage ?? operationResult.ErrorMessage,
                Route = operationResult.Route,
            };

        /// <summary>
        /// The API Response to use at success, with resultData that is custom (mapped).
        /// Currently cannot be used with routing. Add when necessary.
        /// </summary>
        public static ApiResponse<TResult> CreateSuccess(TResult resultData, string route = null) => new()
        {
            Success = true,
            ResultData = resultData,
            Route = route
        };
    }
}