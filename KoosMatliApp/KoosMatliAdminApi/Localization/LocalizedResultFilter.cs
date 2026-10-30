using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace KoosMatliAdminApi.Localization;

public sealed class LocalizedResultFilter(
    IStringLocalizer<Messages> messages)
    : IResultFilter, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        if (user.IsInRole("Admin") &&
            user.HasClaim("accessManaged", "true"))
        {
            var path = context.HttpContext.Request.Path.Value?
                .ToLowerInvariant() ?? "";

            var read = HttpMethods.IsGet(
                context.HttpContext.Request.Method);

            var module =
                path.StartsWith("/api/admin/features/access") ? "roles" :
                path.StartsWith("/api/admin/features/backup") ? "backups" :
                path.StartsWith("/api/admin/dashboard") ? "dashboard" :
                path.StartsWith("/api/admin/users") ? "users" :
                path.StartsWith("/api/admin/applications") ? "applications" :
                path.StartsWith("/api/admin/staff") ? "staff" :
                path.StartsWith("/api/admin/school") ? "school" :
                path.StartsWith("/api/admin/documents") ? "documents" :
                null;

            var permission =
                module == "roles" ? "roles.manage" :
                module == "backups" ? "backups.export" :
                module is null ? null :
                module + (read ? ".read" : ".write");

            if (permission is not null &&
                !user.HasClaim("permission", permission))
            {
                context.Result = new ObjectResult(new
                {
                    message = messages["Forbidden"].Value
                })
                {
                    StatusCode = 403
                };

                return;
            }
        }

        await next();
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult { Value: string text } result)
            result.Value = messages[text].Value;
        else if (context.Result is NotFoundResult)
            context.Result =
                new NotFoundObjectResult(messages["NotFound"].Value);
    }

    public void OnResultExecuted(ResultExecutedContext context) { }
}