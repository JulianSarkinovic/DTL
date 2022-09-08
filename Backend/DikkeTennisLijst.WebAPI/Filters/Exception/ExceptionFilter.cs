using DikkeTennisLijst.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DikkeTennisLijst.WebAPI.Filters.Exception
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unexpected error occurred and was caught by the exception filter.");

            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "application/json";

            var apiResponse = ApiResponse.CreateFailure("Server error occurred: Something went wrong on the server.");
            context.Result = new ObjectResult(apiResponse);
        }
    }
}