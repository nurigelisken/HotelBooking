namespace HotelBooking.Infrastructure.Entities
{
    public class Booking : EntityBase
    {
        public string BookingRef { get; set; }
        public int HotelRoomId { get; set; }
        public HotelRoom HotelRoom { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int NumberOfGuests { get; set; }
    }
}
