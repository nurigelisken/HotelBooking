namespace HotelBooking.Infrastructure.Entities
{
    public class HotelRoom: EntityBase
    {
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int Quantity { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
