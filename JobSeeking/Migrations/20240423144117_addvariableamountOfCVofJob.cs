﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeeking.Migrations
{
    /// <inheritdoc />
    public partial class addvariableamountOfCVofJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "amountOfCV",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amountOfCV",
                table: "Jobs");
        }
    }
}
