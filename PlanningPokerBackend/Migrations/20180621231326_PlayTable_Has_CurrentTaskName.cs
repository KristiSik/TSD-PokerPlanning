using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlanningPokerBackend.Migrations
{
    public partial class PlayTable_Has_CurrentTaskName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskName",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "CurrentTaskName",
                table: "PlayTables",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTaskName",
                table: "PlayTables");

            migrationBuilder.AddColumn<string>(
                name: "TaskName",
                table: "Games",
                nullable: true);
        }
    }
}
