using HotelBooking.Core.Dto;
using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Dto.Response;
using HotelBooking.Core.Repositories.Interfaces;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _dbContext;

        public BookingRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Booking> GetAsync(int id)
        {
            var booking = await _dbContext.Bookings
                                          .Include(f => f.HotelRoom)
                                          .ThenInclude(f => f.Hotel)
                                          .Include(f => f.HotelRoom)
                                          .ThenInclude(f => f.Room)
                                          .FirstOrDefaultAsync(f => f.Id == id);
            return booking ?? new Booking();
        }

        public async Task<Booking> GetAsync(string bookingRef)
        {
            var booking = await _dbContext.Bookings
                                          .Include(f => f.HotelRoom)
                                          .ThenInclude(f => f.Hotel)
                                          .Include(f => f.HotelRoom)
                                          .ThenInclude(f => f.Room)
                                          .FirstOrDefaultAsync(f => f.BookingRef == bookingRef);
            return booking;
        }

        public async Task<IEnumerable<HotelRoom>> GetAvailableHotelRoomsAsync(HotelRoomAvailabiltyRequest request)
        {
            var availableHotelRooms = await _dbContext.HotelRooms
                                                        .Include(hr => hr.Hotel)
                                                        .Include(hr => hr.Room)
                                                        .Include(hr => hr.Bookings)
                                                        .Where(hr =>
                                                            hr.Room.Capacity >= request.NumberOfGuests &&

                                                            // remove rooms if there is any booking on requested date and  room quantity isnt enough
                                                            hr.Bookings.Count(b =>
                                                                (request.CheckinDate <= b.CheckOutDate && b.CheckInDate <= request.CheckoutDate)
                                                            ) < hr.Quantity 
                                                        )
                                                        .ToListAsync();

            return availableHotelRooms;
        }

        public async Task<int> SaveAsync(Booking booking)
        {
            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
            return booking.Id;
        }
    }
}
