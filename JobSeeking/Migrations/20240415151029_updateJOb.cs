using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeeking.Migrations
{
    /// <inheritdoc />
    public partial class updateJOb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "category",
                table: "Jobs",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Jobs",
                newName: "category");
        }
    }
}
