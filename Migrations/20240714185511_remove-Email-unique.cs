using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lombeo.Api.Authorize.Migrations
{
    /// <inheritdoc />
    public partial class removeEmailunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_Email",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Email",
                table: "Bookings",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_Email",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Email",
                table: "Bookings",
                column: "Email",
                unique: true);
        }
    }
}
