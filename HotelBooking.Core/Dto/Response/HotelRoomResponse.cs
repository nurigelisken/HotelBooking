using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Dto.Response
{
    public class HotelRoomResponse
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int Quantity { get; set; }
    }
}
