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
        public async Task<ActionResult<IEnumerable<HotelResponse>>> Search(string query)
        {
            var hotels = await _hotelService.SearchAsync(query);
            return Ok(hotels);
        }

        [HttpGet]
        [Route("{id}", Name = "getHotel")]
        public async Task<ActionResult<HotelResponse>> Get(int id)
        {
            var hotel = await _hotelService.GetAsync(id);
            return Ok(hotel);
        }

        [HttpPost(Name = "save")]
        public async Task<ActionResult<int>> Save(HotelResponse hotelDto)
        {
            return Ok(await _hotelService.SaveAsync(hotelDto));
        }
    }
}
