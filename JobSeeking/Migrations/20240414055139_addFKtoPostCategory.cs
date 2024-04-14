using Microsoft.EntityFrameworkCore.Migrations;



#nullable disable

namespace JobSeeking.Migrations
{
    /// <inheritdoc />
    public partial class addFKtoPostCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployerId",
                table: "PostCategories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_EmployerId",
                table: "PostCategories",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_AspNetUsers_EmployerId",
                table: "PostCategories",
                column: "EmployerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_AspNetUsers_EmployerId",
                table: "PostCategories");

            migrationBuilder.DropIndex(
                name: "IX_PostCategories_EmployerId",
                table: "PostCategories");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "PostCategories");
        }
    }
}
