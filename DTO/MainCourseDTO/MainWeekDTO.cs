namespace Lombeo.Api.Authorize.DTO.MainCourseDTO
{
    public class MainWeekDTO
    {
        public int Week { get; set; }
        public string Title { get; set; }
        public List<MainChapterDTO> Chapters { get; set; }
    }
}
