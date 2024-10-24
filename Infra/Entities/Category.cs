using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Category
    {
        public int Id { get; set; }
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

    public class MultiLangData
    {
        public string Key { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
    }

    public static class CategoryConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                // Bảng
                entity.ToTable("Categories");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Các trường bắt buộc
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                // Mối quan hệ và kiểu dữ liệu
                entity.Property(e => e.Priority)
                    .HasDefaultValue(0);

                entity.Property(e => e.ParentId)
                    .HasDefaultValue(0);

                entity.Property(e => e.CountCourse)
                    .HasDefaultValue(0);

                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedOn)
                    .IsRequired();

                entity.Property(e => e.ModifiedOn)
                    .IsRequired();

                // Map mảng MultiLangData (để lưu trữ JSON)
                entity.Property(e => e.MultiLangData)
                    .HasColumnType("jsonb");
            });
        }
    }
}
