namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class SaveScoreDTO : BaseRequest
    {
        public int QuizId { get; set; }
        public int Score { get; set; }
    }
}
