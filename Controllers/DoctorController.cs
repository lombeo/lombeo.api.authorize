using Lombeo.Api.Authorize.DTO.Common;
using Lombeo.Api.Authorize.DTO.DoctorDTO;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Services.ChildcareService;
using Microsoft.AspNetCore.Mvc;

namespace Lombeo.Api.Authorize.Controllers
{
    [ApiController]
    [Route(RouteApiConstant.BASE_PATH + "/childcare")]
    public class DoctorController : BaseAPIController
    {
        private readonly IChildcareService _childcareService;

        public DoctorController(IChildcareService childcareService)
        {
            _childcareService = childcareService;
        }

        [HttpGet("get-all-doctor")]
        public async Task<ResponseDTO<List<Doctor>>> GetAllDoctor()
        {
            return await HandleException(_childcareService.GetAllDoctor());
        }

        [HttpPost("save-doctor")]
        public async Task<ResponseDTO<int>> SaveDoctor([FromBody] SaveDoctorDTO model)
        {
            return await HandleException(_childcareService.SaveDoctor(model));
        }

        [HttpGet("get-doctor-by-id")]
        public async Task<ResponseDTO<Doctor>> GetDoctorById([FromQuery] int id)
        {
            return await HandleException(_childcareService.GetDoctorById(id));
        }

        [HttpDelete("delete-doctor")]
        public async Task<ResponseDTO<int>> DeleteDoctor([FromQuery] int id)
        {
            return await HandleException(_childcareService.DeleteDoctor(id));
        }

        [HttpGet("get-all-booking")]
        public async Task<ResponseDTO<List<Booking>>> GetAllBooking()
        {
            return await HandleException(_childcareService.GetAllBooking());
        }

        [HttpPost("save-booking")]
        public async Task<ResponseDTO<int>> SaveBooking([FromBody] SaveBookingDTO model)
        {
            return await HandleException(_childcareService.SaveBooking(model));
        }

        [HttpGet("get-booking-by-id")]
        public async Task<ResponseDTO<Booking>> GetBookingById([FromQuery] int id)
        {
            return await HandleException(_childcareService.GetBookingById(id));

        }

        [HttpDelete("delete-booking")]
        public async Task<ResponseDTO<bool>> DeleteBooking([FromQuery] int id)
        {
            return await HandleException(_childcareService.DeleteBooking(id));
        }

        [HttpPost("switch-booking-status")]
        public async Task<ResponseDTO<int>> SwitchBookingStatus([FromQuery] int status, int id)
        {
            return await HandleException(_childcareService.SwitchBookingStatus(status, id));
        }
    }
}
