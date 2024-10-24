using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lombeo.Api.Authorize.Migrations
{
    /// <inheritdoc />
    public partial class updateforcoursemain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "LearningCourses");

            migrationBuilder.DropColumn(
                name: "HasCert",
                table: "LearningCourses");

            migrationBuilder.RenameColumn(
                name: "DiscountPercent",
                table: "LearningCourses",
                newName: "PercentOff");

            migrationBuilder.RenameColumn(
                name: "CourseDescription",
                table: "LearningCourses",
                newName: "SubDescription");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LearningCourses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "LearningCourses");

            migrationBuilder.RenameColumn(
                name: "SubDescription",
                table: "LearningCourses",
                newName: "CourseDescription");

            migrationBuilder.RenameColumn(
                name: "PercentOff",
                table: "LearningCourses",
                newName: "DiscountPercent");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "LearningCourses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasCert",
                table: "LearningCourses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityStatus = table.Column<int>(type: "integer", nullable: false),
                    ActivityTitle = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ActivityType = table.Column<int>(type: "integer", nullable: false),
                    AllowPreview = table.Column<bool>(type: "boolean", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Major = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    SectionId = table.Column<int>(type: "integer", nullable: true),
                    SectionPriority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Introduction = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    NumberSection = table.Column<int>(type: "integer", nullable: false),
                    PercentOff = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Skill = table.Column<string[]>(type: "jsonb", nullable: false),
                    StudyTime = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SubDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    WhatWillYouLearn = table.Column<string[]>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivitiesId = table.Column<List<int>>(type: "jsonb", nullable: false),
                    SectionName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SectionStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });
        }
    }
}
