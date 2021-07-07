using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class FileNameWasAddedInModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_FilesInDb_FileId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "FileId",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte[]>(
                name: "File",
                table: "FilesInDb",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "FilesInDb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_FilesInDb_FileId",
                table: "Questions",
                column: "FileId",
                principalTable: "FilesInDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_FilesInDb_FileId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "FilesInDb");

            migrationBuilder.AlterColumn<int>(
                name: "FileId",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "File",
                table: "FilesInDb",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]));

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_FilesInDb_FileId",
                table: "Questions",
                column: "FileId",
                principalTable: "FilesInDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
