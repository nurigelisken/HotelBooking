using HotelBooking.Core.Dto.Request;
using HotelBooking.Core.Dto.Response;
using HotelBooking.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{bookingRef}", Name = "getBooking")]

        public async Task<ActionResult<ServiceResponse<BookingResponse>>> Get(string bookingRef)
        {
            var booking = await _bookingService.GetAsync(bookingRef);
            return Ok(booking);
        }

        [HttpGet]
        [Route("checkavailability")]
        public async Task<ActionResult<IEnumerable<ServiceResponse<HotelRoomAvailabilityResponse>>>> CheckAvailability([FromQuery] HotelRoomAvailabiltyRequest request)
        {
            var hotelRoomAvailabilityResponse = await _bookingService.GetHotelRoomAvailabilities(request);
            return Ok(hotelRoomAvailabilityResponse);
        }

        [HttpPost(Name = "saveBooking")]
        public async Task<ActionResult<ServiceResponse<BookingResponse>>> Save([FromQuery] HotelRoomBookingRequest hotelRoomBookingRequest)
        {
            var booking = await _bookingService.SaveAsync(hotelRoomBookingRequest);
            return Ok(booking);
        }
    }
}
