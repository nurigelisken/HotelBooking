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

        public async Task<HotelResponse> GetAsync(int id)
        {
            var hotelEntity = await _hotelRepository.GetAsync(id);
            if(hotelEntity == null)
                return new HotelResponse();

            var hotelDto = new HotelResponse { Id = id, Name = hotelEntity.Name };
            return hotelDto;
        }

        public async Task<int> SaveAsync(HotelResponse hotelDto)
        {
            var hotelEntity = new Hotel { Name = hotelDto.Name };
            return await _hotelRepository.SaveAsync(hotelEntity);
        }

        public async Task<IEnumerable<HotelResponse>> SearchAsync(string query)
        {
            var hotelEntities = await _hotelRepository.SearchAsync(query);
            var hotels = hotelEntities.Select(entity => new HotelResponse {
                Id = entity.Id,
                Name = entity.Name,
                HotelRooms  = entity.HotelRooms.Select( hotelRoom => new HotelRoomResponse { 
                    Id= hotelRoom.Id,
                    Quantity = hotelRoom.Quantity,
                    RoomName = hotelRoom.Room.Name
                }).ToList(),
            });

            return hotels;
        }
    }
}
