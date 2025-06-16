using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace money_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TransactionHistories");

            migrationBuilder.RenameColumn(
                name: "Income",
                table: "TransactionHistories",
                newName: "TotalIncome");

            migrationBuilder.RenameColumn(
                name: "Expenses",
                table: "TransactionHistories",
                newName: "TotalExpenses");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "TransactionHistories",
                newName: "LeftoverBalance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalIncome",
                table: "TransactionHistories",
                newName: "Income");

            migrationBuilder.RenameColumn(
                name: "TotalExpenses",
                table: "TransactionHistories",
                newName: "Expenses");

            migrationBuilder.RenameColumn(
                name: "LeftoverBalance",
                table: "TransactionHistories",
                newName: "Balance");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TransactionHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
