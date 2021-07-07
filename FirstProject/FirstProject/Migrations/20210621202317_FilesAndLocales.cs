using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class FilesAndLocales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_LocaleModel_LocaleID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FileModel_AspNetUsers_UserID",
                table: "FileModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocaleModel",
                table: "LocaleModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileModel",
                table: "FileModel");

            migrationBuilder.RenameTable(
                name: "LocaleModel",
                newName: "Locales");

            migrationBuilder.RenameTable(
                name: "FileModel",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_FileModel_UserID",
                table: "Files",
                newName: "IX_Files_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locales",
                table: "Locales",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Locales_LocaleID",
                table: "AspNetUsers",
                column: "LocaleID",
                principalTable: "Locales",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UserID",
                table: "Files",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Locales_LocaleID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UserID",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locales",
                table: "Locales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Locales",
                newName: "LocaleModel");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "FileModel");

            migrationBuilder.RenameIndex(
                name: "IX_Files_UserID",
                table: "FileModel",
                newName: "IX_FileModel_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocaleModel",
                table: "LocaleModel",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileModel",
                table: "FileModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_LocaleModel_LocaleID",
                table: "AspNetUsers",
                column: "LocaleID",
                principalTable: "LocaleModel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileModel_AspNetUsers_UserID",
                table: "FileModel",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
