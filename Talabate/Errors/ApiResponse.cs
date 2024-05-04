
namespace Talabate.Errors
{
    public class ApiResponse
    {
        public int StutusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int stutusCode, string? message=null)
        {
            StutusCode = stutusCode;
            Message = message ?? GetDefultMessageForStutusCode(stutusCode);
        }

        private string? GetDefultMessageForStutusCode(int stutusCode)
        {
            return stutusCode switch
            {
                400 => "A bad request,You Have made",
                401 => "authrized ,You are not",
                404 => "Resource wasn't Found",
                500=>"Errors are the path to the dar side. Errors lead to anger "
              ,
                _ => null,
            };


        }
    }

}
