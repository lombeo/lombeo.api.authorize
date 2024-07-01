using DocumentFormat.OpenXml.Wordprocessing;
using Lombeo.Api.Authorize.DTO.CourseDTO;
using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Services.CacheService;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Services.CourseService
{
	public interface ICourseService
	{
		Task<LearningCourse> GetCourseById(int courseId);
		Task<List<LearningCourse>> GetAllCourse();
		Task<int> SaveCourse(SaveCourseDTO model);
		Task<bool> DeleteCourse(int courseId, int actionBy);
	}

	public class CourseService : ICourseService
	{
		private readonly LombeoAuthorizeContext _context;
		private readonly ICacheService _cacheService;

		public CourseService(LombeoAuthorizeContext context, ICacheService cacheService)
		{
			_context = context;
			_cacheService = cacheService;
		}

		public async Task<bool> DeleteCourse(int courseId, int actionBy)
		{
			if (!IsManager(actionBy)){
				throw new ApplicationException(Message.CommonMessage.NOT_ALLOWED);
			}

			var course = await _context.LearningCourses.FirstOrDefaultAsync(t => t.Id == courseId);
			if (course == null)
			{
				throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
			}

			course.Deleted = true;
			_context.Update(course);

			await _context.SaveChangesAsync();

			_ = _cacheService.DeleteAsync(RedisCacheKey.LIST_COURSE);

			return true;
		}

		public async Task<List<LearningCourse>> GetAllCourse()
		{
			string cacheKey = RedisCacheKey.LIST_COURSE;
			var data = await _cacheService.GetAsync<List<LearningCourse>>(cacheKey);

			if(data == null)
			{
				data = await _context.LearningCourses.Where(t => !t.Deleted).ToListAsync();
				_ = _cacheService.SetAsync(cacheKey, data);
			}

			return data;
		}

		public async Task<LearningCourse> GetCourseById(int courseId)
		{
			var data = await GetAllCourse();
			var course = data.FirstOrDefault(t => t.Id == courseId);
			if(course != null)
			{
				return course;
			}
			throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
		}

		public async Task<int> SaveCourse(SaveCourseDTO model)
		{
			if (!IsManager(model.ActionBy))
			{
				throw new ApplicationException(Message.CommonMessage.NOT_ALLOWED);
			}

			var entity = new LearningCourse()
			{
				Id = 0,
				CreatedAt = DateTime.UtcNow,
				AuthorId = model.ActionBy,
				Deleted = false,
			};

			if (model.Id != 0)
			{
				entity = await _context.LearningCourses.FirstOrDefaultAsync(t => !t.Deleted && t.Id == model.Id);

				if(entity == null)
				{
					throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
				}

				if(entity.AuthorId != model.ActionBy)
				{
					if(!IsManager(model.ActionBy))
					{
						throw new ApplicationException(Message.CommonMessage.NOT_ALLOWED);
					}
				}
			}

			entity.CourseName = model.CourseName;
			entity.CourseDescription = model.CourseDescription;
			entity.Price = model.Price;
			entity.HasCert = model.HasCert;

			if(model.Id != 0)
			{
				_context.Update(entity);
			}
			else
			{
				await _context.AddAsync(entity);
			}

			await _context.SaveChangesAsync();

			_ = _cacheService.DeleteAsync(RedisCacheKey.LIST_COURSE);

			return entity.Id;
		}

		public bool IsManager(int userId)
		{
			IEnumerable<User> allUsers = StaticVariable.UserMemory.ToList();
			var user = allUsers.FirstOrDefault(t => t.Id == userId);
			if (user != null)
			{
				if (user.Role == RoleConstValue.CONTENT_MANAGER)
				{
					return true;
				}
			}
			return false;
		}
	}
}
