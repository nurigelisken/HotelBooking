using HotelBooking.Core.Services.Interfaces;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Entities;
using HotelBooking.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Services
{
    public class SeederService : ISeederService
    {
        private readonly BookingDbContext _dbContext;

        public SeederService(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ResetAsync()
        {

            await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Rooms', RESEED, 0)");
            await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Hotels', RESEED, 0)");
            await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('HotelRooms', RESEED, 0)");

            _dbContext.Rooms.RemoveRange(_dbContext.Rooms);
            _dbContext.Hotels.RemoveRange(_dbContext.Hotels);
            _dbContext.HotelRooms.RemoveRange(_dbContext.HotelRooms);
            _dbContext.Bookings.RemoveRange(_dbContext.Bookings);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedAsync()
        {

            if (!_dbContext.Rooms.Any())
            {
                await _dbContext.Database.OpenConnectionAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Rooms ON");
                _dbContext.Rooms.AddRange(new List<Room>
                {
                    new Room { Id = 1, Type = RoomType.Single, Name = "Single", Capacity = 1 },
                    new Room { Id = 2, Type = RoomType.Double, Name = "Double", Capacity = 2 },
                    new Room { Id = 3, Type = RoomType.Deluxe, Name = "Deluxe", Capacity = 4 }
                });
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Rooms OFF");
                await _dbContext.Database.CloseConnectionAsync();
            }

            if (!_dbContext.Hotels.Any())
            {
                await _dbContext.Database.OpenConnectionAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Hotels ON");
                _dbContext.Hotels.AddRange(new List<Hotel>
                {
                    new Hotel { Id = 1, Name = "Hotel Rengoku" },
                    new Hotel { Id = 2, Name = "Hotel Konohagakure" },
                    new Hotel { Id = 3, Name = "Hotel Tomorrowland" }
                }); 
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Hotels OFF");
                await _dbContext.Database.CloseConnectionAsync();
            }

            if (!_dbContext.HotelRooms.Any())
            {
                await _dbContext.Database.OpenConnectionAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT HotelRooms ON");
                _dbContext.HotelRooms.AddRange(new List<HotelRoom>
                {
                    new HotelRoom { Id = 1, HotelId = 1, RoomId = 1, Quantity = 2 },
                    new HotelRoom { Id = 2, HotelId = 1, RoomId = 2, Quantity = 2 },
                    new HotelRoom { Id = 3, HotelId = 1, RoomId = 3, Quantity = 2 },
                    
                    new HotelRoom { Id = 4, HotelId = 2, RoomId = 1, Quantity = 3 },
                    new HotelRoom { Id = 5, HotelId = 2, RoomId = 2, Quantity = 3 },
                    
                    new HotelRoom { Id = 6, HotelId = 3, RoomId = 1, Quantity = 3 },
                    new HotelRoom { Id = 7, HotelId = 3, RoomId = 3, Quantity = 3 }
                });
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT HotelRooms OFF");
                await _dbContext.Database.CloseConnectionAsync();
            }

            if (!_dbContext.Bookings.Any())
            {
                await _dbContext.Database.OpenConnectionAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Bookings ON");
                _dbContext.Bookings.AddRange(new List<Booking>
                {
                    new Booking { Id = 1, BookingRef = "DummyBookingReference" , HotelRoomId = 1, CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(2), NumberOfGuests = 1 },
                });
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Bookings OFF");
                await _dbContext.Database.CloseConnectionAsync();
            }


        }
    }
}
