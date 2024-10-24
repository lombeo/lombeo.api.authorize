using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class SaveCategoryDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public List<MultiLangData> MultiLangData { get; set; } = new List<MultiLangData>();
        public int ParentId { get; set; }
        public int CountCourse { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
