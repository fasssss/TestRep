using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class Corrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_QuestionId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "FinalResult",
                table: "Polles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Polles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "History");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Polles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                columns: new[] { "QuestionId", "VoteTypeId" });

            migrationBuilder.CreateTable(
                name: "StatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polles_StatusId",
                table: "Polles",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polles_StatusTypes_StatusId",
                table: "Polles",
                column: "StatusId",
                principalTable: "StatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polles_StatusTypes_StatusId",
                table: "Polles");

            migrationBuilder.DropTable(
                name: "StatusTypes");

            migrationBuilder.DropIndex(
                name: "IX_Polles_StatusId",
                table: "Polles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Polles");

            migrationBuilder.AddColumn<string>(
                name: "FinalResult",
                table: "Polles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Polles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_History_QuestionId",
                table: "History",
                column: "QuestionId");
        }
    }
}
