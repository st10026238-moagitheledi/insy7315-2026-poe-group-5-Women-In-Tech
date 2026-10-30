using Grpc.Core;
using Microsoft.Extensions.Localization;
using KoosMatliAdminApi.Localization;
namespace KoosMatliAdminApi.Services;
public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger) //(Microsoft Learn, 2026a).
{
    public async Task InvokeAsync(HttpContext context, IStringLocalizer<Messages> messages)
    {//(Microsoft Learn, 2026d).
        try { await next(context); }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested) { } //(Microsoft Learn, 2026c).
        catch (Exception ex) {
            if (context.Response.HasStarted) throw;
            logger.LogError(ex, "API request failed: {TraceId}", context.TraceIdentifier); //(Microsoft Learn, 2026b).
            context.Response.Clear();
            context.Response.StatusCode = ex is RpcException rpc &&
                (rpc.StatusCode == StatusCode.Unavailable || rpc.StatusCode == StatusCode.DeadlineExceeded) ? 503 : 500; //(gRPC Authors, 2026).
            await context.Response.WriteAsJsonAsync(new {
                message = messages["ServerError"].Value, traceId = context.TraceIdentifier
            });
        }
    }
}

/*Reference [1] Microsoft Corporation, "Write Custom ASP.NET Core Middleware," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write

[2] Microsoft Corporation, "Log in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/

[3] Microsoft Corporation, "Handle Requests with Cancellation Tokens in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/request-cancellation

[4] gRPC Authors, "Error Handling in gRPC," gRPC Documentation, 2026. [Online]. Available: https://grpc.io/docs/guides/error/

[5] Microsoft Corporation, "Globalize and Localize ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localizationList*/
