using Microsoft.EntityFrameworkCore.Migrations;

namespace BeltExam.Migrations
{
    public partial class Migit6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RSVPS_Events_EventId",
                table: "RSVPS");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "RSVPS");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "RSVPS",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RSVPS_Events_EventId",
                table: "RSVPS",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RSVPS_Events_EventId",
                table: "RSVPS");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "RSVPS",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "RSVPS",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RSVPS_Events_EventId",
                table: "RSVPS",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
