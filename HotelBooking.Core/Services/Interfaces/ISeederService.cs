using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Core.Services.Interfaces
{
    public interface ISeederService
    {
        Task SeedAsync();
        Task ResetAsync();
    }
}
