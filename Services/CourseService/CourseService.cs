using Api_Project_Prn.Services.GoogleDriveService;
using DocumentFormat.OpenXml.InkML;
using Lombeo.Api.Authorize.DTO.CourseDTO;
using Lombeo.Api.Authorize.DTO.MainCourseDTO;
using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Infra.Enums;
using Lombeo.Api.Authorize.Services.CacheService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;
using System.Linq;

namespace Lombeo.Api.Authorize.Services.CourseService
{
    public interface ICourseService
    {
        Task<CourseDetailDTO> GetCourseById(int courseId);
        Task<List<AllCourseDTO>> GetAllCourse();
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
        Task<Category> SaveCategory(SaveCategoryDTO model);
        Task<List<Category>> GetAllCategory();
        //Task<Course> CreateCourse(CreateCourseDto dto);
        //Task<Activity> CreateActivity(CreateActivityDto dto);
        //Task<Section> CreateSection(CreateSectionDto dto);
        Task<bool> ReviewCourse(Review model);
        Task<bool> ManageEnrollCourse(EnrollCourseDTO model);
        Task<bool> RequestEnrollCourse(RequestEnrollDTO model);
    }

    public class CourseService : ICourseService
    {
        private readonly LombeoAuthorizeContext _context;
        private readonly ICacheService _cacheService;
        private readonly IGoogleDriveService _googleDriveService;

        public CourseService(LombeoAuthorizeContext context, ICacheService cacheService, IGoogleDriveService googleDriveService)
        {
            _context = context;
            _cacheService = cacheService;
            _googleDriveService = googleDriveService;
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

        public async Task<List<AllCourseDTO>> GetAllCourse()
        {
            var courses = await _context.LearningCourses
                .Where(t => !t.Deleted)
                .Select(c => new AllCourseDTO
                {
                    Id = c.Id,
                    Title = c.CourseName,
                    Description = c.SubDescription, // Assuming SubDescription is a short description
                    Image = c.CourseImage,
                    RegularPrice = (c.Price * (decimal)(1 + (c.PercentOff / 100.0))),
                    DiscountedPrice = c.Price,

                    // Assuming default values for fields not in LearningCourse
                    Rating = _context.Reviews.Where(t => t.CourseId == c.Id).Sum(t => t.Rating) / ((_context.Reviews.Where(t => t.CourseId == c.Id).Count() == 0) ? 1 : (_context.Reviews.Where(t => t.CourseId == c.Id).Count())), // Example placeholder value; replace with actual calculation if available
                    Students = _context.EnrollCourses.Where(t => t.CourseId == c.Id).Count(), // Example placeholder value; replace with actual calculation if available
                    Lectures = (from d in _context.Contents
                                join ch in _context.CourseChapters on d.ChapterId equals ch.Id
                                join w in _context.CourseWeeks on ch.WeekId equals w.Id
                                where w.CourseId == c.Id
                                select d).Count(),  // Example placeholder value; replace with actual calculation if available
                    Hours = (from d in _context.Contents
                             join ch in _context.CourseChapters on d.ChapterId equals ch.Id
                             join w in _context.CourseWeeks on ch.WeekId equals w.Id
                             where w.CourseId == c.Id
                             select d).Sum(t => t.Duration)   // Example placeholder value; replace with actual calculation if available
                })
                .ToListAsync();

            return courses;
        }

        public async Task<CourseDetailDTO> GetCourseById(int courseId)
        {
            var data = await _context.LearningCourses.Where(t => !t.Deleted).ToListAsync();
            var course = data.FirstOrDefault(t => t.Id == courseId);

            if (course != null)
            {
                // Fetch total content count and duration
                var totalContents = await (from content in _context.Contents
                                           join chapter in _context.CourseChapters on content.ChapterId equals chapter.Id
                                           join week in _context.CourseWeeks on chapter.WeekId equals week.Id
                                           where week.CourseId == courseId
                                           select content).CountAsync();

                var totalHour = await (from c in _context.Contents
                                       join ch in _context.CourseChapters on c.ChapterId equals ch.Id
                                       join w in _context.CourseWeeks on ch.WeekId equals w.Id
                                       where w.CourseId == courseId
                                       select c).SumAsync(t => t.Duration);

                // Get course weeks and build the curriculum
                List<MainWeekDTO> curriculum = new List<MainWeekDTO>();
                var courseWeeks = await _context.CourseWeeks.Where(t => t.CourseId == course.Id).ToListAsync();

                foreach (var week in courseWeeks)
                {
                    var chapters = await _context.CourseChapters.Where(t => t.WeekId == week.Id).ToListAsync();
                    var mainChapters = new List<MainChapterDTO>();

                    foreach (var chapter in chapters)
                    {
                        var lectures = await _context.Contents.Where(t => t.ChapterId == chapter.Id).ToListAsync();
                        var mainContents = lectures.Select(content => new MainContentDTO
                        {
                            Type = content.ContentType,
                            Title = content.Title,
                            Duration = content.Duration
                        }).ToList();

                        mainChapters.Add(new MainChapterDTO
                        {
                            Title = chapter.Title,
                            Lectures = mainContents
                        });
                    }

                    curriculum.Add(new MainWeekDTO
                    {
                        Week = week.Index,
                        Title = week.Title,
                        Chapters = mainChapters
                    });
                }

                var Reviews = _context.Reviews.Where(t => t.CourseId != course.Id).Select(t => new MainReviewDTO
                {
                    User = _context.UserProfiles.FirstOrDefault(c => c.UserId == t.ReviewerId).FullName,
                    Rating = t.Rating,
                    Comment = t.Description
                }).ToList();

                // Build CourseDetailDTO with collected data
                var result = new CourseDetailDTO
                {
                    Id = course.Id,
                    Title = course.CourseName,
                    Image = course.CourseImage,
                    Description = course.SubDescription,
                    RegularPrice = (course.Price * (decimal)(1 + (course.PercentOff / 100.0))),
                    DiscountedPrice = course.Price,
                    DiscountPercentage = course.PercentOff,
                    Duration = totalHour,
                    Lectures = totalContents,
                    Students = _context.EnrollCourses.Where(t => t.CourseId == course.Id).Count(),
                    Rating = _context.Reviews.Where(t => t.CourseId == course.Id).Sum(t => t.Rating) / ((_context.Reviews.Where(t => t.CourseId == course.Id).Count() == 0) ? 1 : (_context.Reviews.Where(t => t.CourseId == course.Id).Count())),
                    Introduction = course.Description,
                    LearningObjectives = course.WhatYouWillLearn,
                    Skill = course.Skills,
                    Curriculum = curriculum,
                    Reviews = Reviews // Assuming you have a method to populate MainReviewDTO
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
                Deleted = false,
            };

            if (model.Id != 0)
            {
                entity = await _context.LearningCourses.FirstOrDefaultAsync(t => !t.Deleted && t.Id == model.Id);

                if (entity == null)
                {
                    throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
                }
            }

            entity.CourseName = model.CourseName;
            entity.SubDescription = model.SubDescription;
            entity.Description = model.Description;
            entity.CourseImage = model.CourseImage;
            entity.Skills = model.Skills;
            entity.WhatYouWillLearn = model.WhatYouWillLearn;
            entity.Price = model.Price;
            entity.PercentOff = model.PercentOff;

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
            if (model.ActionBy == 0)
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

            return $"https://img.vietqr.io/image/mbbank-529042003-compact2.jpg?amount={course.Price}&addInfo=Dekiru%20{course.Id}%20{model.ActionBy}&accountName=DO%20DANG%20LONG";
        }

        public async Task<Category> SaveCategory(SaveCategoryDTO model)
        {
            var entity = new Category()
            {
                Name = model.Name,
                Description = model.Description,
                Priority = model.Priority,
                ModifiedOn = model.ModifiedOn,
                MultiLangData = model.MultiLangData,
                ParentId = model.ParentId,
                CountCourse = model.CountCourse,
                Deleted = model.Deleted,
                CreatedOn = model.CreatedOn,
            };

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<bool> ReviewCourse(Review model)
        {
            var flag = await CheckRegisteredCourse(model.CourseId, model.ReviewerId);
            if (flag == false)
            {
                throw new ApplicationException(Message.CourseMessage.NOT_REGISTERED);
            }
            var exist = await _context.Reviews.FirstOrDefaultAsync(t => t.CourseId == model.CourseId && t.ReviewerId == model.ReviewerId && t.Id != model.Id);
            if (exist == null)
            {
                throw new ApplicationException(Message.CourseMessage.ALREADY_REVIEW);
            }

            var review = new Review()
            {
                Id = 0,
                CourseId = model.CourseId,
                ReviewerId = model.ReviewerId,
                Description = model.Description,
                Rating = model.Rating
            };

            var edit = await _context.Reviews.FirstOrDefaultAsync(t => t.Id == model.Id);

            if (edit == null)
            {
                await _context.AddAsync(review);
            }
            else
            {
                review.Id = edit.Id;
                _context.Update(review);
            }
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ManageEnrollCourse(EnrollCourseDTO model)
        {
            if (!IsManager(model.ActionBy))
            {
                throw new ApplicationException(Message.CommonMessage.NOT_ALLOWED);
            }

            var enroll = await _context.EnrollCourses.FirstOrDefaultAsync(t => t.Id == model.EnrollId);

            if (enroll == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
            }

            enroll.UpdatedAt = DateTime.UtcNow;
            enroll.Status = model.Status;

            return true;
        }

        public async Task<bool> RequestEnrollCourse(RequestEnrollDTO model)
        {
            // Validate the request data
            if (model == null || model.ActionBy <= 0 || model.CourseId <= 0)
            {
                throw new ApplicationException("Dữ liệu không hợp lệ!");
            }

            // Check if the user is already enrolled in the course
            var existingEnrollment = await _context.EnrollCourses.FirstOrDefaultAsync(t => t.CourseId == model.CourseId && t.UserId == model.ActionBy);
            if (existingEnrollment != null)
            {
                throw new ApplicationException("Bạn đã đăng ký khóa học này rồi!");
            }

            // Upload the transaction image if provided
            string transactionImgUrl = null;
            if (model.Image != null)
            {
                using var stream = model.Image.OpenReadStream();
                transactionImgUrl = await _googleDriveService.UploadFile(stream, model.Image.FileName, model.Image.ContentType);
            }

            var enrollment = new EnrollCourse
            {
                UserId = model.ActionBy,
                CourseId = model.CourseId,
                InvoiceCode = model.InvoiceCode,
                TransactionImgUrl = transactionImgUrl,
                Status = EnrollStatus.Pending
            };

            await _context.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }



        //public async Task<Course> CreateCourse(CreateCourseDto dto)
        //{
        //    var course = new Course
        //    {
        //        Title = dto.Title,
        //        ImageUrl = dto.ImageUrl,
        //        SubDescription = dto.SubDescription,
        //        Price = dto.Price,
        //        PercentOff = dto.PercentOff,
        //        StudyTime = dto.StudyTime,
        //        NumberSection = dto.NumberSection,
        //        Introduction = dto.Introduction,
        //        WhatWillYouLearn = dto.WhatWillYouLearn,
        //        Skill = dto.Skill,
        //        ActivityId = dto.ActivityId
        //    };

        //    await _context.AddAsync(course);
        //    await _context.SaveChangesAsync();

        //    return course;
        //}

        //public async Task<Activity> CreateActivity(CreateActivityDto dto)
        //{
        //    var activity = new Activity
        //    {
        //        ActivityTitle = dto.ActivityTitle,
        //        Duration = dto.Duration,
        //        Priority = dto.Priority,
        //        SectionPriority = dto.SectionPriority,
        //        ActivityType = dto.ActivityType,
        //        ActivityStatus = dto.ActivityStatus,
        //        Major = dto.Major,
        //        AllowPreview = dto.AllowPreview,
        //        SectionId = dto.SectionId
        //    };

        //    await _context.AddAsync(activity);
        //    await _context.SaveChangesAsync();

        //    return activity;
        //}

        //public async Task<Section> CreateSection(CreateSectionDto dto)
        //{
        //    var section = new Section
        //    {
        //        SectionName = dto.SectionName,
        //        SectionStatus = dto.SectionStatus,
        //        ActivitiesId = dto.ActivitiesId
        //    };

        //    await _context.AddAsync(section);
        //    await _context.SaveChangesAsync();

        //    return section;
        //}
    }
}
