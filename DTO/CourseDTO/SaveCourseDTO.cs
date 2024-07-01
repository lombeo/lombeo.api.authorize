using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
	public class SaveCourseDTO : BaseRequest
	{
		public int Id { get; set; }
		public string CourseName { get; set; }
		public string CourseDescription { get; set; }
		public double Price { get; set; }
		public bool HasCert { get; set; }
		public ContentType ContentType { get; set; }
	}
}
