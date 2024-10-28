using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.MainCourseDTO
{
    public class EnrollRequestDTO
    {
        public string InvoiceCode { get; set; }
        public string User { get; set; }
        public string Course { get; set; }
        public EnrollStatus Status { get; set; }
        public string Image { get; set; }
    }
}
