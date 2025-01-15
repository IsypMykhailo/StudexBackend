using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studex.Migrations
{
    /// <inheritdoc />
    public partial class Topic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Lectures",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Lectures");
        }
    }
}
