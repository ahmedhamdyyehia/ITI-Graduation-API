using System.Collections.Generic;
namespace Api.Errors
{
    public class ApiValidationErrorRespone : ApiResponse
    {

        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorRespone() : base(400)
        {
        }
    }
}
