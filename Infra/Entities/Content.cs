using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Content
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Duration { get; set; }
        public string? VideoUrl { get; set; }
        public int Point { get; set; }
        public int Index { get; set; }
        public ContentType ContentType { get; set; }
    }

    public class ContentConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            // Tên bảng
            builder.ToTable("Contents");

            // Khóa chính
            builder.HasKey(c => c.Id);

            // Thuộc tính bắt buộc và kích thước
            builder.Property(c => c.Title)
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(c => c.Description);

            // Duration sẽ là double
            builder.Property(c => c.Duration)
                   .IsRequired();

            // VideoUrl có thể null
            builder.Property(c => c.VideoUrl)
                   .IsRequired(false);

            builder.Property(c => c.Point);

            builder.Property(c => c.Index)
                   .IsRequired();

            // Ánh xạ ContentType enum
            builder.Property(c => c.ContentType)
                   .HasConversion<int>() // Sử dụng kiểu int để lưu trữ enum
                   .IsRequired();
        }
    }
}
