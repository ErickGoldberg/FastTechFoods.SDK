using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace FastTechFoods.SDK.Middleware
{
    public class GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorId = Guid.NewGuid().ToString();
            logger.LogError(ex, "Erro não tratado | ErrorId: {ErrorId} | Path: {Path} | TraceIdentifier: {Trace}",
                errorId, context.Request.Path, context.TraceIdentifier);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new
            {
                ErrorId = errorId,
                Message = "Ocorreu um erro interno. Tente novamente mais tarde.",
                TraceId = context.TraceIdentifier
            });

            await context.Response.WriteAsync(result);
        }
    }
}