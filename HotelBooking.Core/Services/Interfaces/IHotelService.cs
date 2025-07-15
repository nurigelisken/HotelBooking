using HotelBooking.Core.Dto.Response;
using HotelBooking.Infrastructure.Entities;

namespace HotelBooking.Core.Services.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelResponse>> SearchAsync(string query);

        Task<HotelResponse> GetAsync(int id);

        Task<int> SaveAsync(HotelResponse hotelDto);

    }
}
