using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class CascadeDeletionForFilesInDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_FilesInDb_FileId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_FileId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "FilesInDb",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FilesInDb_QuestionId",
                table: "FilesInDb",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FilesInDb_Questions_QuestionId",
                table: "FilesInDb",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesInDb_Questions_QuestionId",
                table: "FilesInDb");

            migrationBuilder.DropIndex(
                name: "IX_FilesInDb_QuestionId",
                table: "FilesInDb");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "FilesInDb");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_FileId",
                table: "Questions",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_FilesInDb_FileId",
                table: "Questions",
                column: "FileId",
                principalTable: "FilesInDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
