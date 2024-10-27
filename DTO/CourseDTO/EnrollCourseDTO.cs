using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class EnrollCourseDTO : BaseRequest
    {
        public int EnrollId { get; set; }
        public EnrollStatus Status { get; set; }
    }
}
