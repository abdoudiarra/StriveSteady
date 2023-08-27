using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StriveSteady.Migrations
{
    /// <inheritdoc />
    public partial class added_goals_and_subtasks_columns_and_updated_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Goal",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Goal",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "GoalType",
                table: "Goal",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImportanceType",
                table: "Goal",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Goal",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Goal",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subtask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    IsChecked = table.Column<bool>(type: "INTEGER", nullable: false),
                    GoalId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtask_Goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goal_UserId",
                table: "Goal",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtask_GoalId",
                table: "Subtask",
                column: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goal_User_UserId",
                table: "Goal",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goal_User_UserId",
                table: "Goal");

            migrationBuilder.DropTable(
                name: "Subtask");

            migrationBuilder.DropIndex(
                name: "IX_Goal_UserId",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "GoalType",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "ImportanceType",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Goal");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Goal",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
