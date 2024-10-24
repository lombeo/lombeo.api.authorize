namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string SubDescription { get; set; }
        public decimal Price { get; set; }
        public int PercentOff { get; set; }
        public decimal StudyTime { get; set; }
        public int NumberSection { get; set; }
        public string Introduction { get; set; }
        public string[] WhatWillYouLearn { get; set; }
        public string[] Skill { get; set; }
        public int ActivityId { get; set; }
    }

    public class CreateActivityDto
    {
        public string ActivityTitle { get; set; }
        public int Duration { get; set; }
        public int Priority { get; set; }
        public int SectionPriority { get; set; }
        public int ActivityType { get; set; }
        public int ActivityStatus { get; set; }
        public bool Major { get; set; }
        public bool AllowPreview { get; set; }
        public int? SectionId { get; set; }
    }

    public class CreateSectionDto
    {
        public string SectionName { get; set; }
        public int SectionStatus { get; set; }
        public List<int> ActivitiesId { get; set; }
    }

    public class MultiLangDataDto
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

}
