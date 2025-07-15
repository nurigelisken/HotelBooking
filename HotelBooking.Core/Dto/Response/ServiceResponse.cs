using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Dto.Response
{
    public class ServiceResponse<T> where T : class
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ServiceResponse()
        {
            Success = true;
        }

        public ServiceResponse(T data)
        {
            Data = data;
        }
        public ServiceResponse(List<string> errors)
        {
            Message = "Unexpected error is occured, please try again later.";
            Success = false;
            Errors = errors ?? new List<string>();
        }
    }
}
