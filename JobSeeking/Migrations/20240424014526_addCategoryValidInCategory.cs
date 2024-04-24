using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeeking.Migrations
{
    /// <inheritdoc />
    public partial class addCategoryValidInCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "categoryValid",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoryValid",
                table: "Categories");
        }
    }
}
