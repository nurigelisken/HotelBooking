using HotelBooking.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ISeederService _seeder;

        public AdminController(ISeederService seeder)
        {
            _seeder = seeder;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            await _seeder.SeedAsync();
            return Ok("Database is seeded.");
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset()
        {
            await _seeder.ResetAsync();
            return Ok("Database is reset.");
        }
    }
}
