using Noog_api.Enums;

namespace Noog_api.DTOs.BaseResponseDtos
{
    public class BaseResponseDto <T>
    {
        public StatusCodesEnum StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }


    }
}
