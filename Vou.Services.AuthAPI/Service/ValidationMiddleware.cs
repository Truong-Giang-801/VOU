using Vou.Services.AuthAPI.Models.Dto;

namespace Vou.Services.AuthAPI.Service
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline.
            await _next(context);

            // If the response is a 400 error (validation failure)
            if (context.Response.StatusCode == 400 && context.Items["errors"] is Dictionary<string, string[]> errors)
            {
                var response = new ResponeDto
                {
                    IsSuccess = false,
                    Message = "Validation failed",
                    Result = errors
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
