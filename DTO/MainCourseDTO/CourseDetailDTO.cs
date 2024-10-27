namespace Lombeo.Api.Authorize.DTO.MainCourseDTO
{
    public class CourseDetailDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int DiscountPercentage { get; set; }
        public double Duration { get; set; }
        public int Lectures { get; set; }
        public int Students { get; set; }
        public double Rating { get; set; }
        public string Introduction { get; set; }
        public string[] LearningObjectives { get; set; }
        public string[] Skill { get; set; }
        public List<MainWeekDTO> Curriculum { get; set; }
        public List<MainReviewDTO> Reviews { get; set; }
    }
}
