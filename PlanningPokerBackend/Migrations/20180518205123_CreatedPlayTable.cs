using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlanningPokerBackend.Migrations
{
    public partial class CreatedPlayTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "PlayTableId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlayTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayTables_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PlayTableId",
                table: "Users",
                column: "PlayTableId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayTables_AdminId",
                table: "PlayTables",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PlayTables_PlayTableId",
                table: "Users",
                column: "PlayTableId",
                principalTable: "PlayTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PlayTables_PlayTableId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "PlayTables");

            migrationBuilder.DropIndex(
                name: "IX_Users_PlayTableId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PlayTableId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }
    }
}
