using Microsoft.AspNetCore.Diagnostics;
using money_api.Exceptions;

namespace money_api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                if (exception is DuplicateTransactionHistoryException)
                {
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    await context.Response.WriteAsJsonAsync(new { message = exception.Message });
                }
                else if (exception is TransactionHistoryNotFoundException || exception is AccountNotFoundException)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(new { message = exception.Message });
                }
                else if (exception is AccountCreateException accountEx)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = accountEx.Message,
                        errors = accountEx.Errors
                    });
                }
                else if (exception is TransactionDateMismtachException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { message = exception.Message });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
                }
            });
        });
    }
}
