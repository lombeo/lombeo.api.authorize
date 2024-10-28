using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.MainCourseDTO
{
    public class GetEnrollRequestDTO : BaseQueryDTO
    {
        public string? SearchText { get; set; }
        public EnrollStatus Status { get; set; }
    }
}
