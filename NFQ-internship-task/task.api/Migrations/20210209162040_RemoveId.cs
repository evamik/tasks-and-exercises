using Microsoft.EntityFrameworkCore.Migrations;

namespace task.api.Migrations
{
    public partial class RemoveId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId1",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SpecialistId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentSpecialistId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecialistId1",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialistId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SpecialistId",
                table: "Appointments",
                column: "SpecialistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId",
                table: "Appointments",
                column: "SpecialistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SpecialistId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentSpecialistId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SpecialistId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialistId1",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SpecialistId1",
                table: "Appointments",
                column: "SpecialistId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId1",
                table: "Appointments",
                column: "SpecialistId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
