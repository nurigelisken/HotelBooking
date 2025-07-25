using FluentAssertions;
using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Repositories.Interfaces;
using HotelBooking.Core.Services;
using HotelBooking.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelBooking.UnitTests
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<ILogger<BookingService>> _loggerMock;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _loggerMock = new Mock<ILogger<BookingService>>();
            _bookingService = new BookingService(_bookingRepositoryMock.Object, _roomRepositoryMock.Object, _hotelRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBookingResponse_WhenBookingExists()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                HotelRoomId = 1,
                BookingRef = "REF123",
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2,
                HotelRoom = new HotelRoom
                {
                    Hotel = new Hotel { Name = "Test Hotel" },
                    Room = new Room { Name = "Deluxe Room" }
                }
            };
            _bookingRepositoryMock.Setup(repo => repo.GetAsync(1)).ReturnsAsync(booking);
            // Act
            var result = await _bookingService.GetAsync(1);
            // Assert
            result.Should().NotBeNull();
            result.Data.Id.Should().Be(1);
            result.Data.HotelName.Should().Be("Test Hotel");
            result.Data.RoomName.Should().Be("Deluxe Room");
        }
        [Fact]
        public async Task GetAsync_ShouldReturnEmptyResponse_WhenBookingDoesNotExist()
        {
            // Arrange
            _bookingRepositoryMock.Setup(repo => repo.GetAsync(999)).ReturnsAsync((Booking)null);

            // Act
            var result = await _bookingService.GetAsync(999);

            // Assert
            result.Data?.Should().NotBeNull();
            result.Data?.Id.Should().Be(0);
        }
        [Fact]

        public async Task GetAsync_ShouldReturnBookingResponse_WhenBookingRefExists()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                HotelRoomId = 1,
                BookingRef = "REF123",
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2,
                HotelRoom = new HotelRoom
                {
                    Hotel = new Hotel { Name = "Test Hotel" },
                    Room = new Room { Name = "Deluxe Room" }
                }
            };
            _bookingRepositoryMock.Setup(repo => repo.GetAsync("REF123")).ReturnsAsync(booking);
            // Act
            var result = await _bookingService.GetAsync("REF123");
            // Assert
            result.Should().NotBeNull();
            result.Data.Id.Should().Be(1);
            result.Data.BookingRef.Should().Be("REF123");
            result.Data.HotelName.Should().Be("Test Hotel");
            result.Data.RoomName.Should().Be("Deluxe Room");
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmptyResponse_WhenBookingRefDoesNotExist()
        {
            // Arrange
            _bookingRepositoryMock.Setup(repo => repo.GetAsync("NON_EXISTENT_REF")).ReturnsAsync((Booking)null);
            // Act
            var result = await _bookingService.GetAsync("NON_EXISTENT_REF");
            // Assert
            result.Data?.Should().NotBeNull();
            result.Data?.Id.Should().Be(0);
            result.Data?.BookingRef.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmptyResponse_WhenBookingIsNull()
        {
            // Arrange
            _bookingRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((Booking)null);
            // Act
            var result = await _bookingService.GetAsync(1);
            // Assert
            result.Data?.Should().NotBeNull();
            result.Data?.Id.Should().Be(0);
            result.Data?.HotelName.Should().BeNullOrEmpty();
            result.Data?.RoomName.Should().BeNullOrEmpty();

        }

        [Fact]
        public async Task GetAvailableHotelRoomsAsync_ShouldReturnAvailableHotelRooms_WhenRoomsAreAvailable()
        {
            // Arrange
            var request = new HotelRoomAvailabiltyRequest
            {
                CheckinDate = DateTime.Now,
                CheckoutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2
            };
            var availableRooms = new List<HotelRoom>
            {
                new HotelRoom
                {
                    Id = 1,
                    HotelId = 1,
                    RoomId = 1,
                    Quantity = 5,
                    Hotel = new Hotel { Name = "Test Hotel" },
                    Room = new Room { Name = "Single Room", Capacity = 2 }
                }
            };
            _bookingRepositoryMock.Setup(repo => repo.GetAvailableHotelRoomsAsync(request)).ReturnsAsync(availableRooms);
            // Act
            var result = await _bookingService.GetHotelRoomAvailabilities(request);
            // Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.Data.First().HotelName.Should().Be("Test Hotel");
            result.Data.First().RoomName.Should().Be("Single Room");
        }


        [Fact]
        public async Task SaveAsync_ShouldThrowException_WhenRoomCapacityIsNotSuitableForNumberOfGuests()
        {
            // Arrange
            var request = new HotelRoomBookingRequest
            {
                HotelRoomId = 1,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 3 // Exceeds room capacity
            };

            var hotelRoom = new HotelRoom
            {
                Id = 1,
                RoomId = 1,
                HotelId = 1,
                Quantity = 2,
                Room = new Room { Capacity = 2 }
            };
            _hotelRepositoryMock.Setup(repo => repo.GetHotelRoomAsync(request.HotelRoomId)).ReturnsAsync(hotelRoom);
            _roomRepositoryMock.Setup(repo => repo.GetAsync(hotelRoom.RoomId)).ReturnsAsync(hotelRoom.Room);

            // Act
            var result = await _bookingService.SaveAsync(request);

            // Assert
            result.Message.Should().Be($"Sorry, but this room is only available maximum for {hotelRoom.Room.Capacity} guests.");
        }

        [Fact]
        public async Task SaveAsync_ShouldThrowException_WhenBookingIsNotExists()
        {
            // Arrange
            var request = new HotelRoomBookingRequest
            {
                Id = 999, // Non-existing booking ID
                HotelRoomId = 1,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2
            };
            var hotelRoom = new HotelRoom
            {
                Id = 1,
                RoomId = 1,
                HotelId = 1,
                Quantity = 2,
                Room = new Room { Capacity = 2 } // Room capacity is less than number of guests
            };
            _hotelRepositoryMock.Setup(repo => repo.GetHotelRoomAsync(request.HotelRoomId)).ReturnsAsync(hotelRoom);
            _roomRepositoryMock.Setup(repo => repo.GetAsync(hotelRoom.RoomId)).ReturnsAsync(hotelRoom.Room);
            _bookingRepositoryMock.Setup(repo => repo.GetAsync(request.Id)).ReturnsAsync((Booking)null);


            // Act
            var result = await _bookingService.SaveAsync(request);

            // Assert
            result.Message.Should().Be("The booking you are looking for is not found.");

        }

        [Fact]
        public async Task SaveAsync_ShouldThrowException_WhenBookingRoomIsChanged()
        {
            // Arrange
            var request = new HotelRoomBookingRequest
            {
                Id = 999, // Non-existing booking ID
                HotelRoomId = 1,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2
            };
            var hotelRoom = new HotelRoom
            {
                Id = 1,
                RoomId = 1,
                HotelId = 1,
                Quantity = 2,
                Room = new Room { Capacity = 2 } // Room capacity is less than number of guests
            };

            var booking = new Booking
            {
                Id = 999,
                HotelRoomId = 2, // Different hotel room ID
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2
            };

            _hotelRepositoryMock.Setup(repo => repo.GetHotelRoomAsync(request.HotelRoomId)).ReturnsAsync(hotelRoom);
            _roomRepositoryMock.Setup(repo => repo.GetAsync(hotelRoom.RoomId)).ReturnsAsync(hotelRoom.Room);
            _bookingRepositoryMock.Setup(repo => repo.GetAsync(request.Id)).ReturnsAsync(booking);

            // Act
            var result = await _bookingService.SaveAsync(request);

            // Assert
            result.Message.Should().Be("You cant update the room after the booking is confirmed.");

        }
    }
}