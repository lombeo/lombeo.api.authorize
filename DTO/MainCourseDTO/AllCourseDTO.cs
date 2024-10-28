namespace Lombeo.Api.Authorize.DTO.MainCourseDTO
{
    public class AllCourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public int Students { get; set; }
        public int Lectures { get; set; }
        public double Hours { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public bool IsEnroll { get; set; }
    }
}
