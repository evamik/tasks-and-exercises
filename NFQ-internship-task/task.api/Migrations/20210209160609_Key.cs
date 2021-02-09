using Microsoft.EntityFrameworkCore.Migrations;

namespace task.api.Migrations
{
    public partial class Key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId1",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialistId1",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId1",
                table: "Appointments",
                column: "SpecialistId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId1",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialistId1",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_SpecialistId1",
                table: "Appointments",
                column: "SpecialistId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
