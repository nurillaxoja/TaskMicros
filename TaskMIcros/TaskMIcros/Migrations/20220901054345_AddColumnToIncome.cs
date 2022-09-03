using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMIcros.Migrations
{
    public partial class AddColumnToIncome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Commentary",
                table: "Incomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "IncomeLastDate",
                table: "Incomes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commentary",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "IncomeLastDate",
                table: "Incomes");
        }
    }
}
