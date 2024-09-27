using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class QuestionDTO
    {
        public Question Question { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
