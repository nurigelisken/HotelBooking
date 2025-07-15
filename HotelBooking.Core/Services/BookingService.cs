using Azure.Core;
using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Dto.Response;
using HotelBooking.Core.Repositories.Interfaces;
using HotelBooking.Core.Services.Interfaces;
using HotelBooking.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace HotelBooking.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;

        private readonly ILogger<BookingService> _logger;
        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IHotelRepository hotelRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<BookingResponse>> GetAsync(int id)
        {
            try
            {
                var booking = await _bookingRepository.GetAsync(id);
                if (booking == null)
                {
                    return new ServiceResponse<BookingResponse>();
                }
            ;

                var bookingDto = new BookingResponse
                {
                    Id = booking.Id,
                    HotelRoomId = booking.HotelRoomId,
                    BookingRef = booking.BookingRef,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    NumberOfGuests = booking.NumberOfGuests,
                    HotelName = booking.HotelRoom.Hotel.Name,
                    RoomName = booking.HotelRoom.Room.Name,
                };

                return new ServiceResponse<BookingResponse> { Data = bookingDto };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at Service : {nameof(BookingService)} {nameof(BookingService.GetAsync)} /n Message: {ex.Message}");
                return new ServiceResponse<BookingResponse>(new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResponse<BookingResponse>> GetAsync(string bookingRef)
        {
            try
            {
                var booking = await _bookingRepository.GetAsync(bookingRef);
                if (booking == null)
                {
                    return new ServiceResponse<BookingResponse>();
                }
            ;

                var bookingDto = new BookingResponse
                {
                    Id = booking.Id,
                    HotelRoomId = booking.HotelRoomId,
                    BookingRef = booking.BookingRef,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    NumberOfGuests = booking.NumberOfGuests,
                    HotelName = booking.HotelRoom.Hotel.Name,
                    RoomName = booking.HotelRoom.Room.Name,
                };

                return new ServiceResponse<BookingResponse> { Data = bookingDto };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at Service : {nameof(BookingService)} {nameof(BookingService.GetAsync)} /n Message: {ex.Message}");
                return new ServiceResponse<BookingResponse>(new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResponse<IEnumerable<HotelRoomAvailabilityResponse>>> GetHotelRoomAvailabilities(HotelRoomAvailabiltyRequest request)
        {
            try
            {


                var hotelRooms = await _bookingRepository.GetAvailableHotelRoomsAsync(request);

                if (!hotelRooms.Any())
                {
                    return new ServiceResponse<IEnumerable<HotelRoomAvailabilityResponse>>
                    {
                        Message = "No available rooms found for the given criteria.",
                        Data = new List<HotelRoomAvailabilityResponse>()
                    };
                }

                return new ServiceResponse<IEnumerable<HotelRoomAvailabilityResponse>>
                {
                    Data = hotelRooms.Select(s => new HotelRoomAvailabilityResponse
                    {
                        HotelRoomId = s.Id,
                        HotelId = s.HotelId,
                        HotelName = s.Hotel.Name,
                        RoomId = s.RoomId,
                        RoomName = s.Room.Name,
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at Service : {nameof(BookingService)} {nameof(BookingService.GetHotelRoomAvailabilities)} /n Message: {ex.Message}");
                return new ServiceResponse<IEnumerable<HotelRoomAvailabilityResponse>>(new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResponse<BookingResponse>> SaveAsync(HotelRoomBookingRequest hotelRoomBookingRequest)
        {
            try
            {


                var hotelRoom = await _hotelRepository.GetHotelRoomAsync(hotelRoomBookingRequest.HotelRoomId);
                var room = await _roomRepository.GetAsync(hotelRoom.RoomId);
                if (room.Capacity < hotelRoomBookingRequest.NumberOfGuests)
                {
                    return new ServiceResponse<BookingResponse>
                    {
                        Message = $"Sorry, but this room is only available maximum for {room.Capacity} guests.",
                    };
                }


                if (hotelRoomBookingRequest.Id > 0)
                {
                    var existingBooking = await _bookingRepository.GetAsync(hotelRoomBookingRequest.Id);
                    if (existingBooking == null)
                    {
                        return new ServiceResponse<BookingResponse>
                        {
                            Message = "The booking you are looking for is not found.",
                        };
                    }

                    if (existingBooking.HotelRoomId != hotelRoomBookingRequest.HotelRoomId)
                    {
                        return new ServiceResponse<BookingResponse>
                        {
                            Message = "You cant update the room after the booking is confirmed.",
                        };
                    }

                    // if user wants to change the date of booking or number of guests, check the room is still available
                    if (existingBooking.CheckInDate != hotelRoomBookingRequest.CheckInDate || existingBooking.CheckOutDate != hotelRoomBookingRequest.CheckOutDate)
                    {
                        return new ServiceResponse<BookingResponse>
                        {
                            Message = "Unfortunately, we are not able to change your booking dates due to no available rooms.",
                        };
                    }

                    existingBooking.CheckInDate = hotelRoomBookingRequest.CheckInDate;
                    existingBooking.CheckOutDate = hotelRoomBookingRequest.CheckOutDate;
                    existingBooking.NumberOfGuests = hotelRoomBookingRequest.NumberOfGuests;

                    var bookingId = await _bookingRepository.SaveAsync(existingBooking);
                    hotelRoomBookingRequest.Id = bookingId;
                }
                else
                {
                    var request = new HotelRoomAvailabiltyRequest
                    {
                        CheckinDate = hotelRoomBookingRequest.CheckInDate,
                        CheckoutDate = hotelRoomBookingRequest.CheckOutDate,
                        NumberOfGuests = hotelRoomBookingRequest.NumberOfGuests,
                    };

                    // if the booking isnt created then check the room is occupied by someone else or not
                    var availableHotelRoomsResponse = await GetHotelRoomAvailabilities(request);
                    if (availableHotelRoomsResponse != null && !availableHotelRoomsResponse.Data.Any(f => f.HotelRoomId == hotelRoomBookingRequest.HotelRoomId))
                    {
                        return new ServiceResponse<BookingResponse>
                        {
                            Message = "Sorry, the room you requested to book is not available at the moment.",
                        };
                    }

                    var booking = new Booking();
                    booking.BookingRef = GenerateBookingRef();
                    booking.HotelRoomId = hotelRoomBookingRequest.HotelRoomId;
                    booking.CheckInDate = hotelRoomBookingRequest.CheckInDate;
                    booking.CheckOutDate = hotelRoomBookingRequest.CheckOutDate;
                    booking.NumberOfGuests = hotelRoomBookingRequest.NumberOfGuests;
                    var bookingId = await _bookingRepository.SaveAsync(booking);
                    hotelRoomBookingRequest.Id = bookingId;
                }

                var savedBookingResponse = await GetAsync(hotelRoomBookingRequest.Id);
                return new ServiceResponse<BookingResponse> { Data = savedBookingResponse.Data };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at Service : {nameof(BookingService)} {nameof(BookingService.SaveAsync)} /n Message: {ex.Message}");
                return new ServiceResponse<BookingResponse>(new List<string> { ex.Message });
            }
        }

        private string GenerateBookingRef()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var randomPart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            return $"B-{datePart}-{randomPart}";
        }
    }
}
