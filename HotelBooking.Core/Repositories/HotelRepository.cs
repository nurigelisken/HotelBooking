using HotelBooking.Core.Repositories.Interfaces;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBooking.Core.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly BookingDbContext _dbContext;
        public HotelRepository(BookingDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Hotel> GetAsync(int id)
        {
            return await _dbContext.Hotels.Where(f => f.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<HotelRoom> GetHotelRoomAsync(int id)
        {
            return await _dbContext.HotelRooms.Where(f => f.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<int> SaveAsync(Hotel hotel)
        {
            await _dbContext.Hotels.AddAsync(hotel);
            await _dbContext.SaveChangesAsync();
            return hotel.Id;
        }

        public async Task<IEnumerable<Hotel>> SearchAsync(string query)
        {
            return await _dbContext.Hotels
                                    .Include(f => f.HotelRooms)
                                    .ThenInclude(f => f.Room)
                                    .Where(f => f.Name.Contains(query))
                                    .ToListAsync();
        }
    }
}
