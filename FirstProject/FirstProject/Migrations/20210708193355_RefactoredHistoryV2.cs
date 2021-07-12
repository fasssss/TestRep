using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstProject.Migrations
{
    public partial class RefactoredHistoryV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Questions_QuestionId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_VotesTypes_VoteTypeId",
                table: "History");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_VoteTypeId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "VoteTypeId",
                table: "History");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "VotesInPollsHistory");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "VotesInPollsHistory",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PollHistoryId",
                table: "VotesInPollsHistory",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "VoteName",
                table: "VotesInPollsHistory",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VotesInPollsHistory",
                table: "VotesInPollsHistory",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PollsHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PollName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollsHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotesInPollsHistory_PollHistoryId",
                table: "VotesInPollsHistory",
                column: "PollHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_VotesInPollsHistory_PollsHistory_PollHistoryId",
                table: "VotesInPollsHistory",
                column: "PollHistoryId",
                principalTable: "PollsHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotesInPollsHistory_PollsHistory_PollHistoryId",
                table: "VotesInPollsHistory");

            migrationBuilder.DropTable(
                name: "PollsHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VotesInPollsHistory",
                table: "VotesInPollsHistory");

            migrationBuilder.DropIndex(
                name: "IX_VotesInPollsHistory_PollHistoryId",
                table: "VotesInPollsHistory");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VotesInPollsHistory");

            migrationBuilder.DropColumn(
                name: "PollHistoryId",
                table: "VotesInPollsHistory");

            migrationBuilder.DropColumn(
                name: "VoteName",
                table: "VotesInPollsHistory");

            migrationBuilder.RenameTable(
                name: "VotesInPollsHistory",
                newName: "History");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VoteTypeId",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                columns: new[] { "QuestionId", "VoteTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_History_VoteTypeId",
                table: "History",
                column: "VoteTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Questions_QuestionId",
                table: "History",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_VotesTypes_VoteTypeId",
                table: "History",
                column: "VoteTypeId",
                principalTable: "VotesTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
