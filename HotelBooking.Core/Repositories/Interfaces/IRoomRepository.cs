using HotelBooking.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> GetAsync(int id);
    }
}
