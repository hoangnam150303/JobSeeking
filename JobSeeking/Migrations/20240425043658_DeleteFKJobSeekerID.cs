using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeeking.Migrations
{
    /// <inheritdoc />
    public partial class DeleteFKJobSeekerID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyCV_AspNetUsers_JobSeekerId",
                table: "ApplyCV");

            migrationBuilder.DropIndex(
                name: "IX_ApplyCV_JobSeekerId",
                table: "ApplyCV");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "ApplyCV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobSeekerId",
                table: "ApplyCV",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyCV_JobSeekerId",
                table: "ApplyCV",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyCV_AspNetUsers_JobSeekerId",
                table: "ApplyCV",
                column: "JobSeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
