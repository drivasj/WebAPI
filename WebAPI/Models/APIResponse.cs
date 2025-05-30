using System.Net;

namespace WebAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }

        public bool IsSucces {  get; set; } =  true;

        public List<string> ErrorsMessages { get; set; }

        public object Result { get; set; }
    }
}
