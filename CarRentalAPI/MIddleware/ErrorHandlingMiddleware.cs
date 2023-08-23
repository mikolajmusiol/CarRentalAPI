using CarRentalAPI.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CarRentalAPI.MIddleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
                await next.Invoke(context);
            }
			catch (NotFoundException notFound)
			{
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFound.Message);
            }
			catch (Exception)
			{
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
