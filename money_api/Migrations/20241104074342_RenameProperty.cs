using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace money_api.Migrations
{
    /// <inheritdoc />
    public partial class RenameProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeftoverBalance",
                table: "TransactionHistories",
                newName: "Balance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "TransactionHistories",
                newName: "LeftoverBalance");
        }
    }
}
