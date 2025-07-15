using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Dto.Response
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public  string BookingRef { get; set; }
        public int HotelRoomId { get; set; }
        public string HotelName { get; set; }
        public string RoomName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int NumberOfGuests { get; set; }

    }
}
