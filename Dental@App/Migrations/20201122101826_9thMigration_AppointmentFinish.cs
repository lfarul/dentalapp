using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dental_App.Migrations
{
    public partial class _9thMigration_AppointmentFinish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointments",
                newName: "AppointmentStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentFinish",
                table: "Appointments",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentFinish",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "AppointmentStart",
                table: "Appointments",
                newName: "AppointmentDate");
        }
    }
}
