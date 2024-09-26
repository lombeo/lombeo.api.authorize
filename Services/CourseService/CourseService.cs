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
		Task<LearningCourseDTO> GetCourseById(int courseId);
		Task<List<LearningCourse>> GetAllCourse();
		Task<int> SaveCourse(SaveCourseDTO model);
		Task<bool> DeleteCourse(int courseId, int actionBy);
		Task<List<CourseWeek>> GetAllCourseWeeksById(int id, int actionBy);
		Task<List<CourseChapter>> GetAllCourseChaptersById(int id, int actionBy);
		Task<List<Reading>> GetAllReadingsById(int id, int actionBy);
		Task<List<Video>> GetAllVideosById(int id, int actionBy);
		Task<List<Quiz>> GetAllQuizById(int id, int actionBy);
		Task<List<Question>> GetAllQuestionsById(int id, int actionBy);
		Task<List<Answer>> GetAllAnswersById(int id, int actionBy);
		Task<bool> CheckRegisteredCourse(int courseId, int actionBy);
		Task<bool> GetListRegisteredCourse(int actionBy);
		Task<Score> SaveScore(SaveScoreDTO model);
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

		public async Task<LearningCourseDTO> GetCourseById(int courseId)
		{
			var data = await GetAllCourse();
			var course = data.FirstOrDefault(t => t.Id == courseId);
			double learningHour = 0;
			int lectureAmmount = 0;

			if(course != null)
			{
                learningHour += await _context.Readings.SumAsync(t => t.Duration);
                learningHour += await _context.Videos.SumAsync(t => t.Duration);
                learningHour += await _context.Quizzes.SumAsync(t => t.TimeLimit);

                lectureAmmount += await _context.Readings.CountAsync(t => t.Id != 0);
                lectureAmmount += await _context.Videos.CountAsync(t => t.Id != 0);
                lectureAmmount += await _context.Quizzes.CountAsync(t => t.Id != 0);

                var result = new LearningCourseDTO()
				{
					CourseName = course.CourseName,
					CourseDescription = course.CourseDescription,
					AuthorId = course.AuthorId,
					Skills = course.Skills,
					WhatYouWillLearn = course.WhatYouWillLearn,
					HasCert = course.HasCert,
					Price = course.Price,
					LearningHour = learningHour,
					LectureAmmount = lectureAmmount,
					UpdatedAt = course.UpdatedAt,
					CreatedAt = course.CreatedAt,
					Deleted = course.Deleted
				};

				return result;

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
				UpdatedAt = DateTime.UtcNow,
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
			entity.Skills = model.Skills;
			entity.WhatYouWillLearn = model.WhatYouWillLearn;

			if(model.Id != 0)
			{
				_context.Update(entity);
			}
			else
			{
                entity.CreatedAt = DateTime.UtcNow;
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

        public async Task<List<CourseWeek>> GetAllCourseWeeksById(int id)
        {
            var data = await _context.CourseWeeks.Where(t => t.CourseId == id).ToListAsync();
			return data;
        }

        public async Task<List<CourseChapter>> GetAllCourseChaptersById(int id)
        {
            return await _context.CourseChapters.Where(t => t.WeekId == id).ToListAsync();
        }

        public async Task<List<Reading>> GetAllReadingsById(int id)
        {
            return await _context.Readings.Where(t => t.CourseChapterId == id).ToListAsync();
        }

        public async Task<List<Video>> GetAllVideosById(int id)
        {
            return await _context.Videos.Where(t => t.CourseChapterId == id).ToListAsync();
        }

        public async Task<List<Quiz>> GetAllQuizById(int id)
        {
            return await _context.Quizzes.Where(t => t.CourseChapterId == id).ToListAsync();
        }

        public async Task<List<Question>> GetAllQuestionsById(int id)
        {
            return await _context.Questions.Where(t => t.QuizId == id).ToListAsync();
        }

        public async Task<List<Answer>> GetAllAnswersById(int id)
        {
            return await _context.Answers.Where(t => t.QuestionId == id).ToListAsync();
        }

        public Task<List<CourseWeek>> GetAllCourseWeeksById(int id, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseChapter>> GetAllCourseChaptersById(int id, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reading>> GetAllReadingsById(int id, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<List<Video>> GetAllVideosById(int id, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<List<Quiz>> GetAllQuizById(int id, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<List<Question>> GetAllQuestionsById(int id, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<List<Answer>> GetAllAnswersById(int id, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckRegisteredCourse(int courseId, int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetListRegisteredCourse(int actionBy)
        {
            throw new NotImplementedException();
        }

        public Task<Score> SaveScore(SaveScoreDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
