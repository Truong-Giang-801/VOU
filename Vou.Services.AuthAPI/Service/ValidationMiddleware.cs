using System.Text.Json;
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
            // Capture the original body stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream to capture the response
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                try
                {
                    // Call the next delegate/middleware in the pipeline
                    await _next(context);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var body = new StreamReader(memoryStream).ReadToEnd();
                    context.Response.Body = originalBodyStream;
                    // If the response is a 400 error (validation failure)
                    if (context.Response.StatusCode == StatusCodes.Status400BadRequest && !string.IsNullOrEmpty(body) && body.Contains("required", StringComparison.OrdinalIgnoreCase))
                    {
                        // Set the response body back to the original stream


                        // Check if the response body contains a "required" field error
                       
                            // Create a custom response for missing fields
                            var response = new ResponeDto
                            {
                                IsSuccess = false,
                                Message = "Vui lòng điền đầy đủ các trường bắt buộc.",
                                Result = null
                            };

                            context.Response.ContentType = "application/json";
                            // Use a new memory stream to serialize the custom response
                            using (var newMemoryStream = new MemoryStream())
                            {
                                await JsonSerializer.SerializeAsync(newMemoryStream, response, new JsonSerializerOptions
                                {
                                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                    WriteIndented = true // Optional: for pretty-printing
                                });
                                newMemoryStream.Seek(0, SeekOrigin.Begin);
                                await newMemoryStream.CopyToAsync(originalBodyStream);
                            }
                        
                    }
                    else
                    {
                        // If not a 400 error, just copy the response to the original stream
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        await memoryStream.CopyToAsync(originalBodyStream);
                    }
                }
                catch (Exception ex)
                {
                    // Handle unexpected exceptions
                    context.Response.Body = originalBodyStream;
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    var response = new ResponeDto
                    {
                        IsSuccess = false,
                        Message = $"An unexpected error occurred: {ex.Message}"
                    };

                    context.Response.ContentType = "application/json";
                    await JsonSerializer.SerializeAsync(originalBodyStream, response, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    });
                }
            }
        }

    }
}
