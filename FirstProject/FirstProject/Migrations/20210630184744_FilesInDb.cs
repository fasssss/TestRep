using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class FilesInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FilesInDb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File = table.Column<byte[]>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesInDb", x => x.Id);
                });

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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_FilesInDb_FileId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "FilesInDb");

            migrationBuilder.DropIndex(
                name: "IX_Questions_FileId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Questions");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "Questions",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
