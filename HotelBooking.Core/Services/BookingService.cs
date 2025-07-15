using Azure.Core;
using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Dto.Response;
using HotelBooking.Core.Repositories.Interfaces;
using HotelBooking.Core.Services.Interfaces;
using HotelBooking.Infrastructure.Entities;

namespace HotelBooking.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IHotelRepository hotelRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task<BookingResponse> GetAsync(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);
            if (booking == null)
                return new BookingResponse();

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

            return bookingDto;
        }

        public async Task<BookingResponse> GetAsync(string bookingRef)
        {
            var booking = await _bookingRepository.GetAsync(bookingRef);
            if (booking == null)
                return new BookingResponse();

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

            return bookingDto;
        }

        public async Task<IEnumerable<HotelRoomAvailabilityResponse>> GetHotelRoomAvailabilities(HotelRoomAvailabiltyRequest request)
        {
            var hotelRooms = await _bookingRepository.GetAvailableHotelRoomsAsync(request);
            if (hotelRooms == null)
                return new List<HotelRoomAvailabilityResponse>();

            return hotelRooms.Select(s => new HotelRoomAvailabilityResponse
            {
                HotelRoomId = s.Id,
                HotelId = s.HotelId,
                HotelName = s.Hotel.Name,
                RoomId = s.RoomId,
                RoomName = s.Room.Name,
            }).ToList();
        }

        public async Task<BookingResponse> SaveAsync(HotelRoomBookingRequest hotelRoomBookingRequest)
        {
            var hotelRoom = await _hotelRepository.GetHotelRoomAsync(hotelRoomBookingRequest.HotelRoomId);
            var room = await _roomRepository.GetAsync(hotelRoom.RoomId);
            if (room.Capacity < hotelRoomBookingRequest.NumberOfGuests)
                throw new Exception($"Sorry, but this room is only available maximum for {room.Capacity}");


            if (hotelRoomBookingRequest.Id > 0)
            {
                var existingBooking = await _bookingRepository.GetAsync(hotelRoomBookingRequest.Id);
                if (existingBooking == null)
                    throw new Exception("The booking you are looking for is not found.");

                if (existingBooking.HotelRoomId != hotelRoomBookingRequest.HotelRoomId)
                    throw new Exception("You cant update the room after the booking is confirmed.");

                // if user wants to change the date of booking or number of guests, check the room is still available
                if (existingBooking.CheckInDate != hotelRoomBookingRequest.CheckInDate || existingBooking.CheckOutDate != hotelRoomBookingRequest.CheckOutDate)
                    throw new Exception("Unfortunately, we are not able to change your booking dates due to no available rooms.");

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
                var availableHotelRooms = await GetHotelRoomAvailabilities(request);
                if (!availableHotelRooms.Any(f => f.HotelRoomId == hotelRoomBookingRequest.HotelRoomId))
                    throw new Exception("Sorry, the room you requested to book is not available at the moment.");

                var booking = new Booking();
                booking.BookingRef = GenerateBookingRef();
                booking.HotelRoomId = hotelRoomBookingRequest.HotelRoomId;
                booking.CheckInDate = hotelRoomBookingRequest.CheckInDate;
                booking.CheckOutDate = hotelRoomBookingRequest.CheckOutDate;
                booking.NumberOfGuests = hotelRoomBookingRequest.NumberOfGuests;
                var bookingId = await _bookingRepository.SaveAsync(booking);
                hotelRoomBookingRequest.Id = bookingId;
            }

            var savedBooking = await GetAsync(hotelRoomBookingRequest.Id);
            return savedBooking;
        }

        private string GenerateBookingRef()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var randomPart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            return $"B-{datePart}-{randomPart}";
        }
    }
}
