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
    public class RoomRepository : IRoomRepository
    {
        private readonly BookingDbContext _dbContext;

        public RoomRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Room> GetAsync(int id)
        {
            return await _dbContext.Rooms.Where(f => f.Id == id).FirstOrDefaultAsync();
        }
    }
}
