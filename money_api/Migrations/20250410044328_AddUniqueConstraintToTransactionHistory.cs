using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace money_api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToTransactionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionHistories_UserId",
                table: "TransactionHistories");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistories_UserId_Month_Year",
                table: "TransactionHistories",
                columns: new[] { "UserId", "Month", "Year" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionHistories_UserId_Month_Year",
                table: "TransactionHistories");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistories_UserId",
                table: "TransactionHistories",
                column: "UserId");
        }
    }
}
