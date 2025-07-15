using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Dto.Response;

namespace HotelBooking.Core.Services.Interfaces
{
    public interface IBookingService
    {
        Task<ServiceResponse<BookingResponse>> GetAsync(int id);
        Task<ServiceResponse<BookingResponse>> GetAsync(string bookingRef);
        Task<ServiceResponse<IEnumerable<HotelRoomAvailabilityResponse>>> GetHotelRoomAvailabilities(HotelRoomAvailabiltyRequest request);

        Task<ServiceResponse<BookingResponse>> SaveAsync(HotelRoomBookingRequest hotelRoomBookingRequest);

    }
}
