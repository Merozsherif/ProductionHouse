using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionHouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectImageFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFolder",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFolder",
                table: "Projects");
        }
    }
}
