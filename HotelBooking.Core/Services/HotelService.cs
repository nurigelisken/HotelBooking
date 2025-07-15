using Azure.Core.Pipeline;
using HotelBooking.Core.Dto;
using HotelBooking.Core.Dto.Response;
using HotelBooking.Core.Repositories.Interfaces;
using HotelBooking.Core.Services.Interfaces;
using HotelBooking.Infrastructure.Entities;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Core.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<BookingService> _logger;

        public HotelService(IHotelRepository hotelRepository, ILogger<BookingService> logger)
        {
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<HotelResponse>> GetAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Error at Service : {nameof(HotelService)} {nameof(HotelService.GetAsync)} /n Message: {ex.Message}");
                return new ServiceResponse<HotelResponse>(new List<string> { ex.Message });
            }
        }

        public async Task<int> SaveAsync(string name)
        {
            try
            {
                var hotelEntity = new Hotel { Name = name };
                return await _hotelRepository.SaveAsync(hotelEntity);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error at Service : {nameof(HotelService)} {nameof(HotelService.GetAsync)} /n Message: {ex.Message}");
                return 0;
            }
        }

        public async Task<ServiceResponse<IEnumerable<HotelResponse>>> SearchAsync(string query)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Error at Service : {nameof(HotelService)} {nameof(HotelService.SearchAsync)} /n Message: {ex.Message}");
                return new ServiceResponse<IEnumerable<HotelResponse>>(new List<string> { ex.Message });
            }
        }
    }
}
