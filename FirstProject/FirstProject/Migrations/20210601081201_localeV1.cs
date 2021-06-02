using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class localeV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_LocaleModel_LocaleID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocaleModel",
                table: "LocaleModel");

            migrationBuilder.RenameTable(
                name: "LocaleModel",
                newName: "Locales");

            migrationBuilder.AlterColumn<string>(
                name: "LocaleID",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Locales",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locales",
                table: "Locales",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Locales_LocaleID",
                table: "AspNetUsers",
                column: "LocaleID",
                principalTable: "Locales",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Locales_LocaleID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locales",
                table: "Locales");

            migrationBuilder.RenameTable(
                name: "Locales",
                newName: "LocaleModel");

            migrationBuilder.AlterColumn<int>(
                name: "LocaleID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LocaleModel",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocaleModel",
                table: "LocaleModel",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_LocaleModel_LocaleID",
                table: "AspNetUsers",
                column: "LocaleID",
                principalTable: "LocaleModel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
