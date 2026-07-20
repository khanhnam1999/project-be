using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialPayment",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "InstallmentMonth",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "LoanAmount",
                table: "Contracts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InitialPayment",
                table: "Contracts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InstallmentMonth",
                table: "Contracts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LoanAmount",
                table: "Contracts",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
