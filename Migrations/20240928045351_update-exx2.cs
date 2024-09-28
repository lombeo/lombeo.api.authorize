using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lombeo.Api.Authorize.Migrations
{
    /// <inheritdoc />
    public partial class updateexx2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Time",
                table: "Scores",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CourseImage",
                table: "LearningCourses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "CourseImage",
                table: "LearningCourses");
        }
    }
}
