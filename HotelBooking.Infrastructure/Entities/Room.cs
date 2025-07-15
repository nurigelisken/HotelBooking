using HotelBooking.Infrastructure.Enums;

namespace HotelBooking.Infrastructure.Entities
{
    public class Room : EntityBase
    {
        public RoomType Type { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public ICollection<HotelRoom> HotelRooms { get; set; }

    }
}
