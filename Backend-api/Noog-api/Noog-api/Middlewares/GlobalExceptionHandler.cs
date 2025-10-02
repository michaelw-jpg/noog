using Microsoft.AspNetCore.Diagnostics;
using Noog_api.DTOs.BaseResponseDtos;

namespace Noog_api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {

        //global exception handler handles all unhandled exceptions in the application, defaults to this if no try catch is used
        // returns a standardized error response with appropriate status code and message
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            var responseDto = new BaseResponseDto<string>
            {
                
                Message = "An unexpected error occurred.",
                Data = exception.Message,
                StatusCode = exception switch
                {
                    ArgumentNullException => Enums.StatusCodesEnum.BadRequest,
                    ArgumentException => Enums.StatusCodesEnum.BadRequest,
                    KeyNotFoundException => Enums.StatusCodesEnum.NotFound,
                    UnauthorizedAccessException => Enums.StatusCodesEnum.Unauthorized,
                    _ => Enums.StatusCodesEnum.ServerError

                }
            };
            
            await Helpers.ApiResponseHelper.WriteJsonAsync(context, responseDto, cancellationToken);
            return true;

        }
    }
}
