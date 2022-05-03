using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlackOverload.Data.Migrations
{
    public partial class AddNewClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reputation",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Answer_AnswerId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AnswerId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Comment");

            migrationBuilder.CreateTable(
                name: "AnswerComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerComment_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerComment_AnswerId",
                table: "AnswerComment",
                column: "AnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerComment");

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AnswerId",
                table: "Comment",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Answer_AnswerId",
                table: "Comment",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id");
        }
    }
}
