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
		public async Task<ResponseDTO<LearningCourseDTO>> GetCourseById([FromQuery] int courseId)
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

        [HttpGet("get-all-course-week")]
        [AllowAnonymous]
        public async Task<ResponseDTO<List<CourseWeek>>> GetAllCourseWeeksById([FromQuery] int id)
        {
            return await HandleException(_courseService.GetAllCourseWeeksById(id, UserId));
        }

        [HttpGet("get-all-course-chapter")]
        [AllowAnonymous]
        public async Task<ResponseDTO<List<CourseChapter>>> GetAllCourseChaptersById([FromQuery] int id)
        {
            return await HandleException(_courseService.GetAllCourseChaptersById(id));
        }

        [HttpGet("get-question-for-quiz")]
        [AllowAnonymous]
        public async Task<ResponseDTO<List<QuestionDTO>>> GetQuestionForQuiz([FromQuery] int id)
        {
            return await HandleException(_courseService.GetQuestionForQuiz(id));
        }

        [HttpGet("check-registered-course")]
        public async Task<ResponseDTO<bool>> CheckRegisteredCourse([FromQuery] int id)
        {
            return await HandleException(_courseService.CheckRegisteredCourse(id, UserId));
        }

        [HttpGet("check-list-registered-course")]
        public async Task<ResponseDTO<List<LearningCourse>>> GetListRegisteredCourseByUserId()
        {
            return await HandleException(_courseService.GetListRegisteredCourseByUserId(UserId));
        }

        [HttpPost("save-score")]
        public async Task<ResponseDTO<Score>> SaveScore([FromBody] SaveScoreDTO model)
        {
            model.ActionBy = UserId;
            return await HandleException(_courseService.SaveScore(model));
        }
        [HttpPost("register-course")]
        public async Task<ResponseDTO<bool>> RegisterCourse([FromQuery] int courseId)
        {
            return await HandleException(_courseService.RegisterCourse(courseId, UserId));
        }
        [HttpGet("get-list-content-by-chap-id")]
        public async Task<ResponseDTO<List<Content>>> GetListContentsByChapId([FromQuery] int chapId)
        {
            return await HandleException(_courseService.GetListContentsByChapId(chapId));
        }
    }
}
