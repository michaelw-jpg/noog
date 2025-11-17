namespace Noog_mvc.Helpers
{
    public class ServiceResultHelper<T>
    {
        public bool Succsess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResultHelper<T> SuccessResult(T data, string message = "")
        {
            return new ServiceResultHelper<T>
            {
                Succsess = true,
                Data = data,
                Message = message
            };
        }

        public static ServiceResultHelper<T> FailiureResult(string message, List<string> errors)
        {
            return new ServiceResultHelper<T>
            {
                Succsess = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
