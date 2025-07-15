using HotelBooking.Infrastructure.Entities;
using HotelBooking.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Core.Dto.Response;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {

        private readonly IHotelService _hotelService;
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<HotelResponse>>>> Search(string query)
        {
            var hotels = await _hotelService.SearchAsync(query);
            return Ok(hotels);
        }

        [HttpGet]
        [Route("{id}", Name = "getHotel")]
        public async Task<ActionResult<ServiceResponse<HotelResponse>>> Get(int id)
        {
            var hotel = await _hotelService.GetAsync(id);
            return Ok(hotel);
        }

        [HttpPost(Name = "save")]
        public async Task<ActionResult<int>> Save([FromQuery] string name)
        {
            return Ok(await _hotelService.SaveAsync(name));
        }
    }
}
