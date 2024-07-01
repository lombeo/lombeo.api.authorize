using Lombeo.Api.Authorize.DTO.Common;
using Lombeo.Api.Authorize.DTO.CourseDTO;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Services.CourseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lombeo.Api.Authorize.Controllers
{
	[ApiController]
	[Route(RouteApiConstant.BASE_PATH + "/course")]
	[Authorize]
	public class CourseController : BaseAPIController
	{
		private readonly ICourseService _courseService;

		public CourseController(ICourseService courseService)
		{
			_courseService = courseService;
		}

		[HttpGet("get-course-by-id")]
		[AllowAnonymous]
		public async Task<ResponseDTO<LearningCourse>> GetCourseById([FromQuery] int courseId)
		{
			return await HandleException(_courseService.GetCourseById(courseId));
		}

		[HttpGet("get-all-course")]
		[AllowAnonymous]
		public async Task<ResponseDTO<List<LearningCourse>>> GetAllCourse()
		{
			return await HandleException(_courseService.GetAllCourse());
		}

		[HttpPost("save-course")]
		public async Task<ResponseDTO<int>> SaveCourse([FromBody] SaveCourseDTO model)
		{
			model.ActionBy = UserId;
			return await HandleException(_courseService.SaveCourse(model));
		}

		[HttpDelete("delete-course")]
		public async Task<ResponseDTO<bool>> DeleteCourse([FromQuery] int courseId)
		{
			return await HandleException(_courseService.DeleteCourse(courseId, UserId));
		}

	}
}
