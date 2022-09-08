namespace DikkeTennisLijst.Core.Shared.Results
{
    public class OperationResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public Exception? Exception { get; init; }
        public string? Route { get; init; }

        protected OperationResult()
        { }

        /// <summary>
        /// The operation result to use when the operation was a success.
        /// </summary>
        public static OperationResult CreateSuccess(string? route = null) => new()
        {
            Success = true,
            Route = route
        };

        /// <summary>
        /// The operation result that will generally be used if the server did something wrong and an exception was thrown.
        /// The type supplied is only used to conform to callback expectations.
        /// </summary>
        /// <param name="ex">
        /// The exception to include, and of which the message to use if no custom error message is provided.
        /// </param>
        public static OperationResult CreateFailure(Exception ex, string? customErrorMessage, string? route = null) => new()
        {
            Success = false,
            ErrorMessage = customErrorMessage ?? ex.Message,
            Exception = ex,
            Route = route
        };

        /// <summary>
        /// The operation result that will generally be used if the user did something wrong.
        /// The type supplied is only used to conform to callback expectations.
        /// </summary>
        /// <param name="errorMessage"></param>
        public static OperationResult CreateFailure(string? errorMessage = null, string? route = null) => new()
        {
            Success = false,
            ErrorMessage = errorMessage,
            Route = route
        };
    }

    public sealed class OperationResult<TResult> : OperationResult
    {
        public TResult? ResultData { get; set; }

        private OperationResult()
        { }

        /// <summary>
        /// The operation result to use when the operation was a success.
        /// </summary>
        public static OperationResult<TResult> CreateSuccess(TResult result, string? route = null) => new()
        {
            Success = true,
            ResultData = result,
            Route = route
        };

        /// <summary>
        /// The operation result that will generally be used if the server did something wrong and an exception was thrown.
        /// The type supplied is only used to conform to callback expectations.
        /// </summary>
        /// <param name="ex">
        /// The exception to include, and of which the message to use if no custom error message is provided.
        /// </param>
        public new static OperationResult<TResult> CreateFailure(
            Exception ex, string? customErrorMessage = null, string? route = null) => new()
            {
                Success = false,
                ErrorMessage = customErrorMessage ?? ex.Message,
                Exception = ex,
                Route = route
            };

        /// <summary>
        /// The operation result that will generally be used if the user did something wrong.
        /// The type supplied is only used to conform to callback expectations.
        /// </summary>
        /// <param name="errorMessage"></param>
        public new static OperationResult<TResult> CreateFailure(string? errorMessage = null, string? route = null) => new()
        {
            Success = false,
            ErrorMessage = errorMessage,
            Route = route
        };
    }
}