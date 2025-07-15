using HotelBooking.Infrastructure.Entities;
using HotelBooking.Core.Repositories.Interfaces;
using HotelBooking.Core.Services.Interfaces;
using HotelBooking.Core.Dto;
using Azure.Core.Pipeline;
using HotelBooking.Core.Dto.Response;

namespace HotelBooking.Core.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<ServiceResponse<HotelResponse>> GetAsync(int id)
        {
            var hotelEntity = await _hotelRepository.GetAsync(id);
            if (hotelEntity == null)
            {
                return new ServiceResponse<HotelResponse>
                {
                    Success = false,
                    Message = "Hotel not found"
                };
            }

            var hotelDto = new HotelResponse { Id = id, Name = hotelEntity.Name };
            return new ServiceResponse<HotelResponse>
            {
                Data = hotelDto
            };
        }

        public async Task<int> SaveAsync(string name)
        {
            var hotelEntity = new Hotel { Name = name };
            return await _hotelRepository.SaveAsync(hotelEntity);
        }

        public async Task<ServiceResponse<IEnumerable<HotelResponse>>> SearchAsync(string query)
        {
            var hotelEntities = await _hotelRepository.SearchAsync(query);
            var hotels = hotelEntities.Select(entity => new HotelResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                HotelRooms = entity.HotelRooms.Select(hotelRoom => new HotelRoomResponse
                {
                    Id = hotelRoom.Id,
                    Quantity = hotelRoom.Quantity,
                    RoomName = hotelRoom.Room.Name
                }).ToList(),
            });

            return new ServiceResponse<IEnumerable<HotelResponse>>
            {
                Data = hotels
            };
        }
    }
}
