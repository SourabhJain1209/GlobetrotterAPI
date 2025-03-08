namespace GlobetrotterAPIs.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }   // Indicates success or failure
        public string Message { get; set; }   // API message
        public T Data { get; set; }           // The actual response data
        public string Error { get; set; }     // Error details (if applicable)

        public ApiResponse() { }

        public ApiResponse(bool isSuccess, string message, T data, string error = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Error = error;
        }
    }


}
