using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Change_account_resident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Contracts_ContractId",
                table: "Residents");

            migrationBuilder.DropIndex(
                name: "IX_Residents_ContractId",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Residents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "Residents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Residents_ContractId",
                table: "Residents",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Contracts_ContractId",
                table: "Residents",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "ContractId");
        }
    }
}
