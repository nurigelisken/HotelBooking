using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Dto.Response;

namespace HotelBooking.Core.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> GetAsync(int id);
        Task<BookingResponse> GetAsync(string bookingRef);
        Task<IEnumerable<HotelRoomAvailabilityResponse>> GetHotelRoomAvailabilities(HotelRoomAvailabiltyRequest request);

        Task<BookingResponse> SaveAsync(HotelRoomBookingRequest hotelRoomBookingRequest);

    }
}
