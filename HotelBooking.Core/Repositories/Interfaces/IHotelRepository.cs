using HotelBooking.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Repositories.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> SearchAsync(string query);

        Task<Hotel> GetAsync(int id);
        Task<int> SaveAsync(Hotel hotel);

        Task<HotelRoom> GetHotelRoomAsync(int id);

    }
}
