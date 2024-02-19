using Microsoft.AspNetCore.Http;

namespace Game.Common.Exceptions
{
    public class ExceptionsMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next) 
        {
            try 
            {
                await next(httpContext);
            } 
            catch(Exception ex) 
            {
                await HandlerException(httpContext, ex);
            }
        }

        public static Task HandlerException(HttpContext httpContext, Exception ex) 
        {

            int statusCode = StatusCodes.Status500InternalServerError;

            statusCode = ex switch
            {
                NotFoundException _ => StatusCodes.Status404NotFound,
                BadRequestException _ => StatusCodes.Status400BadRequest,
                DatabaseException _ => StatusCodes.Status503ServiceUnavailable,
                PublishError _ => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status500InternalServerError

            };
            
            var errorResp = new ErrorMap
            {
                StatusCode = statusCode,
                Message = ex.Message,
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            return httpContext.Response.WriteAsync(errorResp.Message);
        }
    }
}
