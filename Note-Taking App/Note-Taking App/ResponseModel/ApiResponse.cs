namespace Note_Taking_App.ResponseModel
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public List<string> Errors { get; set; } = new();
       

        public ApiResponse(bool success, string message, T data, List<string> errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }

       
        public static ApiResponse<T> SuccessResponse(T data, string message = "The request was completed successfully.") =>
            new(true, message, data);

        public static ApiResponse<T> ErrorResponse(object error, string message = "An unexpected error has occurred.")
        {
            var errorList = error switch
            {
                string e => new List<string> { e },
                List<string> list => list,
                _ => new List<string> { "An unknown error format was provided." }
            };

            return new ApiResponse<T>(false, message, default, errorList);
        }

    }

}

