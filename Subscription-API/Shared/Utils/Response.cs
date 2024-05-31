namespace Shared.Utils
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Response() { }

        public Response SetSuccess(object? data = null, string? message = "")
        {
            Success = true;
            Message = message;
            Data = data ?? Array.Empty<object>();
            return this;
        }

        public Response SetFailure(string message)
        {
            Success = false;
            Message = message;
            Data = Array.Empty<object>();
            return this;
        }
    }
}
