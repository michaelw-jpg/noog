using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Enums;
using System.Text.Json;

namespace Noog_api.Helpers
{
    public static class ApiResponseHelper
    {

        //Method to write JSON response directly to HttpContext used for global exception handling
        public static async Task WriteJsonAsync<T>(HttpContext context, BaseResponseDto<T> response, CancellationToken cancellationToken = default)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });
            await context.Response.WriteAsync(json, cancellationToken);
        }


        public static ActionResult<T> ToActionResult<T>(BaseResponseDto<T> response)
        {
            if (response == null)
                return new StatusCodeResult(500);

            // Determine empty result
            bool isEmpty = (response.Data == null);


            return response.StatusCode switch
            {

                StatusCodesEnum.Success => response.Data == null
                    ? new OkObjectResult(Array.Empty<T>()) // empty but 200 OK
                    : new OkObjectResult(response.Data),

                StatusCodesEnum.NoContent => new NoContentResult(),
                StatusCodesEnum.Created => new CreatedResult("", response.Data),
                StatusCodesEnum.BadRequest => new BadRequestObjectResult(response.Message),
                StatusCodesEnum.Unauthorized => new UnauthorizedResult(),
                StatusCodesEnum.Forbidden => new StatusCodeResult(403),
                StatusCodesEnum.NotFound => new NotFoundObjectResult(response.Message),
                StatusCodesEnum.Conflict => new ConflictObjectResult(response.Message),
                StatusCodesEnum.ServerError => new ObjectResult(response.Message) { StatusCode = 500 },
                _ => new ObjectResult(response.Message) { StatusCode = 520 },
            };
        }
    }
}
