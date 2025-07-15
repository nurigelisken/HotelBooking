using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Dto.Response
{
    public class HotelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<HotelRoomResponse> HotelRooms { get; set; }
    }
}
