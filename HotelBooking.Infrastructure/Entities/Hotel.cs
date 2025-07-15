namespace HotelBooking.Infrastructure.Entities
{
    public class Hotel : EntityBase
    {
        public string Name { get; set; }
        public ICollection<HotelRoom> HotelRooms { get; set; }

    }
}
