using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace money_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMonthYearProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthYear",
                table: "TransactionHistories");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "TransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "TransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "TransactionHistories");

            migrationBuilder.AddColumn<DateTime>(
                name: "MonthYear",
                table: "TransactionHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
