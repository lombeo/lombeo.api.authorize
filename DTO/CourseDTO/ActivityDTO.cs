using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class ActivityDTO
    {
        public CourseWeek Week { get; set; }
        public List<CourseChapter> Chapters { get; set; }
        public List<Content> Activities { get; set; }
    }
}
