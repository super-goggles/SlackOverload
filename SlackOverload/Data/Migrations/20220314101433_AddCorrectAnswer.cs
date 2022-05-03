using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlackOverload.Data.Migrations
{
    public partial class AddCorrectAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswer",
                table: "Answer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Answer");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswer",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
