using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeltExam.Migrations
{
    public partial class Migit4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RSVPS_Activities_ActivityId",
                table: "RSVPS");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_RSVPS_ActivityId",
                table: "RSVPS");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "RSVPS",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    DateAndTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RSVPS_EventId",
                table: "RSVPS",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RSVPS_Events_EventId",
                table: "RSVPS",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RSVPS_Events_EventId",
                table: "RSVPS");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropIndex(
                name: "IX_RSVPS_EventId",
                table: "RSVPS");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "RSVPS");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DateAndTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RSVPS_ActivityId",
                table: "RSVPS",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                table: "Activities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RSVPS_Activities_ActivityId",
                table: "RSVPS",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
