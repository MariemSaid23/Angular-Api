namespace Talabate.Errors
{
    public class ApiExcecutionResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExcecutionResponse(int statuscode,string? message =null ,string? details = null)
            :base(statuscode,message)
        {
            Details = details;
        }
    }
}
