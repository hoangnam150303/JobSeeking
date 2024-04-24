using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeeking.Migrations
{
    /// <inheritdoc />
    public partial class addVariablesStatusOfApplyCV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ApplyCV",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ApplyCV");
        }
    }
}
