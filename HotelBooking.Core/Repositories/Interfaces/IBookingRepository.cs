using HotelBooking.Core.Dto;
using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Dto.Response;
using HotelBooking.Infrastructure.Entities;

namespace HotelBooking.Core.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> GetAsync(int id);  
        Task<Booking> GetAsync(string bookingRef);

        Task<IEnumerable<HotelRoom>> GetAvailableHotelRoomsAsync(HotelRoomAvailabiltyRequest request);

        Task<int> SaveAsync(Booking booking);
    }
}
