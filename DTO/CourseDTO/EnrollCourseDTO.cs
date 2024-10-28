using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class EnrollCourseDTO : BaseRequest
    {
        public string InvoiceCode { get; set; }
        public EnrollStatus Status { get; set; }
    }
}
