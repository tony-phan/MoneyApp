using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace money_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLeftoverBalanceFromTransactionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftoverBalance",
                table: "TransactionHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LeftoverBalance",
                table: "TransactionHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
