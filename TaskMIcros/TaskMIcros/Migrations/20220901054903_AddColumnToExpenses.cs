using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMIcros.Migrations
{
    public partial class AddColumnToExpenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Commentary",
                table: "Expenses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpenseLastDate",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commentary",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseLastDate",
                table: "Expenses");
        }
    }
}
