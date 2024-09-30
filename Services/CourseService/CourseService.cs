using DocumentFormat.OpenXml.InkML;
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
        Task<List<CourseChapter>> GetAllCourseChaptersById(int id);
        Task<List<Question>> GetAllQuestionsById(int id);
        Task<List<Answer>> GetAllAnswersById(int id);
        Task<bool> CheckRegisteredCourse(int courseId, int actionBy);
        Task<List<LearningCourse>> GetListRegisteredCourseByUserId(int actionBy);
        Task<Score> SaveScore(SaveScoreDTO model);
        Task<List<QuestionDTO>> GetQuestionForQuiz(int quizId);
        Task<bool> RegisterCourse(int courseId, int actionBy);
        Task<List<Content>> GetListContentsByChapId(int chapId);
        Task<string> CreateTransaction(TransactionDTO model);
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

            if (data == null)
            {
                data = await _context.LearningCourses.Where(t => !t.Deleted).ToListAsync();
                _ = _cacheService.SetAsync(cacheKey, data);
            }

            return data;
        }

        public async Task<LearningCourseDTO> GetCourseById(int courseId)
        {
            _ = _cacheService.DeleteAsync(RedisCacheKey.LIST_COURSE);
            var data = await GetAllCourse();
            var course = data.FirstOrDefault(t => t.Id == courseId);
            double learningHour = 0;
            int lectureAmmount = 0;

            if (course != null)
            {
                var totalContents = (from c in _context.Contents
                                     join ch in _context.CourseChapters on c.ChapterId equals ch.Id
                                     join w in _context.CourseWeeks on ch.WeekId equals w.Id
                                     where w.CourseId == courseId
                                     select c).Count();

                var totalHour = (from c in _context.Contents
                                 join ch in _context.CourseChapters on c.ChapterId equals ch.Id
                                 join w in _context.CourseWeeks on ch.WeekId equals w.Id
                                 where w.CourseId == courseId
                                 select c).Sum(t => t.Duration);

                var result = new LearningCourseDTO()
                {
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    AuthorId = course.AuthorId,
                    Skills = course.Skills,
                    CourseImage = course.CourseImage,
                    WhatYouWillLearn = course.WhatYouWillLearn,
                    HasCert = course.HasCert,
                    Price = course.Price,
                    LearningHour = totalHour,
                    LectureAmmount = totalContents,
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

                if (entity == null)
                {
                    throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
                }

                if (entity.AuthorId != model.ActionBy)
                {
                    if (!IsManager(model.ActionBy))
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

            if (model.Id != 0)
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

        public async Task<List<CourseWeek>> GetAllCourseWeeksById(int id, int actionBy)
        {
            if (actionBy == 0 || await CheckRegisteredCourse(id, actionBy) == false)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_ALLOWED);
            }
            var data = await _context.CourseWeeks.Where(t => t.CourseId == id).ToListAsync();
            return data;
        }

        public async Task<List<CourseChapter>> GetAllCourseChaptersById(int id)
        {
            return await _context.CourseChapters.Where(t => t.WeekId == id).ToListAsync();
        }

        public async Task<List<Question>> GetAllQuestionsById(int id)
        {
            return await _context.Questions.Where(t => t.QuizId == id).ToListAsync();
        }

        public async Task<List<Answer>> GetAllAnswersById(int id)
        {
            return await _context.Answers.Where(t => t.QuestionId == id).ToListAsync();
        }

        public async Task<bool> CheckRegisteredCourse(int courseId, int actionBy)
        {
            return await _context.UserCourses.FirstOrDefaultAsync(t => t.UserId == actionBy && t.LearningCourseId == courseId) != null;
        }

        public async Task<Score> SaveScore(SaveScoreDTO model)
        {
            if (model.ActionBy == 0)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
            }

            var entity = new Score()
            {
                Id = 0,
                UserId = model.ActionBy,
                QuizId = model.QuizId
            };

            var existScore = await _context.Scores.FirstOrDefaultAsync(t => t.UserId == model.ActionBy && t.QuizId == model.QuizId);

            if (existScore != null)
            {
                entity.Id = existScore.Id;
                entity.Point = model.Score;
                entity.Time = model.Score;

                _context.Update(entity);
            }
            else
            {
                entity.Point = model.Score;
                await _context.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<LearningCourse>> GetListRegisteredCourseByUserId(int actionBy)
        {
            var data = from uc in _context.UserCourses
                       join lc in _context.LearningCourses
                       on uc.LearningCourseId equals lc.Id
                       where uc.UserId == actionBy
                       select lc;
            return await data.ToListAsync();
        }

        public async Task<List<QuestionDTO>> GetQuestionForQuiz(int quizId)
        {
            var questions = _context.Questions.Where(t => t.QuizId == quizId).ToList();

            var questionDTOs = questions.Select(q => new QuestionDTO
            {
                Question = q,
                Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList()
            }).ToList();

            return questionDTOs;
        }

        public async Task<bool> RegisterCourse(int courseId, int actionBy)
        {
            if (actionBy == 0)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_AUTHEN);
            }

            if (await CheckRegisteredCourse(courseId, actionBy) == true)
            {
                throw new ApplicationException("Bạn đã đăng ký khóa học này rồi!");
            }

            await _context.AddAsync(new UserCourses()
            {
                UserId = actionBy,
                LearningCourseId = courseId,
                RegisteredAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Content>> GetListContentsByChapId(int chapId)
        {
            var chap = await _context.CourseChapters.FirstOrDefaultAsync(t => t.Id == chapId);
            if (chap == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
            }
            var content = _context.Contents.Where(t => t.ChapterId == chapId).ToList();
            return content;
        }

        public async Task<string> CreateTransaction(TransactionDTO model)
        {
            if(model.ActionBy == 0)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_AUTHEN);
            }

            var course = await _context.LearningCourses.FirstOrDefaultAsync(t => t.Id == model.CourseId);
            if (course == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
            }
            var trans = new Transaction()
            {
                CourseId = course.Id,
                CourseName = course.CourseName,
                Price = course.Price,
                UserId = model.ActionBy
            };

            await _context.AddAsync(trans);
            await _context.SaveChangesAsync();

            return $"https://img.vietqr.io/image/mbbank-529042003-compact2.jpg?amount={course.Price}&addInfo=Dekiru%20{course.Id}-{model.ActionBy}&accountName=DO%20DANG%20LONG";
        }
    }
}
