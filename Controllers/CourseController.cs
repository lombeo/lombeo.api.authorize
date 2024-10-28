using Lombeo.Api.Authorize.DTO.Common;
using Lombeo.Api.Authorize.DTO.CourseDTO;
using Lombeo.Api.Authorize.DTO.MainCourseDTO;
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
		public async Task<ResponseDTO<CourseDetailDTO>> GetCourseById([FromQuery] int courseId)
		{
			return await HandleException(_courseService.GetCourseById(courseId, UserId));
		}

		[HttpGet("get-all-course")]
		[AllowAnonymous]
		public async Task<ResponseDTO<List<AllCourseDTO>>> GetAllCourse()
		{
			return await HandleException(_courseService.GetAllCourse(UserId));
		}

        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<int> Test()
        {
            return UserId;
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

        [HttpPost("create-and-get-qr-transaction")]
        public async Task<ResponseDTO<string>> CreateTransaction([FromQuery] TransactionDTO model)
        {
            model.ActionBy = UserId;
            return await HandleException(_courseService.CreateTransaction(model));
        }

        [AllowAnonymous]
        [HttpPost("save-category")]
        public async Task<ResponseDTO<Category>> SaveCategory([FromBody] SaveCategoryDTO model)
        {
            return await HandleException(_courseService.SaveCategory(model));
        }

        [AllowAnonymous]
        [HttpGet("get-category")]
        public async Task<ResponseDTO<List<Category>>> GetAllCategory()
        {
            return await HandleException(_courseService.GetAllCategory());
        }

        [HttpPost("request-enroll-course")]
        public async Task<ResponseDTO<bool>> RequestEnrollCourse([FromQuery] RequestEnrollDTO model)
        {
            model.ActionBy = UserId;
            return await HandleException(_courseService.RequestEnrollCourse(model));
        }

        [HttpPost("manage-enroll-course")]
        public async Task<ResponseDTO<bool>> ManageEnrollCourse([FromQuery] EnrollCourseDTO model)
        {
            model.ActionBy = UserId;
            return await HandleException(_courseService.ManageEnrollCourse(model));
        }

        [HttpGet("get-course-revenue")]
        public async Task<ResponseDTO<List<CourseRevenueDTO>>> GetCourseRevenues()
        {
            return await HandleException(_courseService.GetCourseRevenues(UserId));
        }

        [HttpGet("get-enroll-request")]
        public async Task<ResponseDTO<List<EnrollRequestDTO>>> GetEnrollRequest()
        {
            return await HandleException(_courseService.GetEnrollRequest(UserId));
        }

        //[AllowAnonymous]
        //[HttpPost("create-course")]
        //public async Task<ResponseDTO<Course>> CreateCourse(CreateCourseDto model)
        //{
        //    return await HandleException(_courseService.CreateCourse(model));
        //}

        //[AllowAnonymous]
        //[HttpPost("create-activity")]
        //public async Task<ResponseDTO<Activity>> CreateActivity(CreateActivityDto model)
        //{
        //    return await HandleException(_courseService.CreateActivity(model));
        //}

        //[AllowAnonymous]
        //[HttpPost("create-section")]
        //public async Task<ResponseDTO<Section>> CreateSection(CreateSectionDto model)
        //{
        //    return await HandleException(_courseService.CreateSection(model));
        //}
    }
}
