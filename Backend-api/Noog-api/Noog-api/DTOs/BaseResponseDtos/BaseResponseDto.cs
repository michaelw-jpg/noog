using Noog_api.Enums;

namespace Noog_api.DTOs.BaseResponseDtos
{
    public class BaseResponseDto <T>
    {
        public StatusCodesEnum StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public BaseResponseDto()
        {
        }

        // Constructor with parameters
        public BaseResponseDto(StatusCodesEnum statusCode, string message, T? data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

    }
}
