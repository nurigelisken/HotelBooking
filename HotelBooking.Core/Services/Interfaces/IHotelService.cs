using HotelBooking.Core.Dto.Response;
using HotelBooking.Infrastructure.Entities;

namespace HotelBooking.Core.Services.Interfaces
{
    public interface IHotelService
    {
        Task<ServiceResponse<IEnumerable<HotelResponse>>> SearchAsync(string query);

        Task<ServiceResponse<HotelResponse>> GetAsync(int id);

        Task<int> SaveAsync(string name);

    }
}
