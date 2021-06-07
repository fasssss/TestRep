using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class localeV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Locales_LocaleID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locales",
                table: "Locales");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Locales",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "LocaleID",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locales",
                table: "Locales",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Locales_LocaleID",
                table: "AspNetUsers",
                column: "LocaleID",
                principalTable: "Locales",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Locales_LocaleID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locales",
                table: "Locales");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Locales",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LocaleID",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(int));

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
    }
}
