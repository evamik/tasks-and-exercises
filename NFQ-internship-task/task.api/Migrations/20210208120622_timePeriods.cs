using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace task.api.Migrations
{
    public partial class timePeriods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailableTimePeriod",
                columns: table => new
                {
                    AvailableTimePeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentSpecialistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableTimePeriod", x => x.AvailableTimePeriodId);
                    table.ForeignKey(
                        name: "FK_AvailableTimePeriod_AppointmentSpecialists_AppointmentSpecialistId",
                        column: x => x.AppointmentSpecialistId,
                        principalTable: "AppointmentSpecialists",
                        principalColumn: "AppointmentSpecialistId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailableTimePeriod_AppointmentSpecialistId",
                table: "AvailableTimePeriod",
                column: "AppointmentSpecialistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableTimePeriod");
        }
    }
}
