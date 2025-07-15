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
        public bool Success { get; set; } = true;
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
    }
}
