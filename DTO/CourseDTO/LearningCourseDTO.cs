using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class LearningCourseDTO : CommonEntity
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int AuthorId { get; set; }
        public string[] Skills { get; set; }
        public string[] WhatYouWillLearn { get; set; }
        public bool HasCert { get; set; }
        public decimal Price { get; set; }
        public double LearningHour { get; set; }
        public int LectureAmmount { get; set; }
        public string CourseImage { get; set; }
    }
}
