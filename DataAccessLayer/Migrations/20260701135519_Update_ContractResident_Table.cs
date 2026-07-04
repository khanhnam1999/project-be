using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Update_ContractResident_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractResident_Contracts_ContractId",
                table: "ContractResident");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractResident_Residents_ResidentId",
                table: "ContractResident");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractResident",
                table: "ContractResident");

            migrationBuilder.RenameTable(
                name: "ContractResident",
                newName: "ContractResidents");

            migrationBuilder.RenameIndex(
                name: "IX_ContractResident_ResidentId",
                table: "ContractResidents",
                newName: "IX_ContractResidents_ResidentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractResidents",
                table: "ContractResidents",
                columns: new[] { "ContractId", "ResidentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractResidents_Contracts_ContractId",
                table: "ContractResidents",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractResidents_Residents_ResidentId",
                table: "ContractResidents",
                column: "ResidentId",
                principalTable: "Residents",
                principalColumn: "ResidentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractResidents_Contracts_ContractId",
                table: "ContractResidents");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractResidents_Residents_ResidentId",
                table: "ContractResidents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractResidents",
                table: "ContractResidents");

            migrationBuilder.RenameTable(
                name: "ContractResidents",
                newName: "ContractResident");

            migrationBuilder.RenameIndex(
                name: "IX_ContractResidents_ResidentId",
                table: "ContractResident",
                newName: "IX_ContractResident_ResidentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractResident",
                table: "ContractResident",
                columns: new[] { "ContractId", "ResidentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractResident_Contracts_ContractId",
                table: "ContractResident",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractResident_Residents_ResidentId",
                table: "ContractResident",
                column: "ResidentId",
                principalTable: "Residents",
                principalColumn: "ResidentId");
        }
    }
}
