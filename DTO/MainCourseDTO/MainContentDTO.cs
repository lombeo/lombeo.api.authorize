using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.MainCourseDTO
{
    public class MainContentDTO
    {
        public ContentType Type { get; set; }
        public string Title { get; set; }
        public double Duration { get; set; }
    }
}
