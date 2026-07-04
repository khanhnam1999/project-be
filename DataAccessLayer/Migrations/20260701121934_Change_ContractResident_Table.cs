using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Change_ContractResident_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Residents_ResidentId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Apartments_ApartmentId",
                table: "Residents");

            migrationBuilder.DropIndex(
                name: "IX_Residents_ApartmentId",
                table: "Residents");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ResidentId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "ResidentType",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "ResidentId",
                table: "Contracts");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "Residents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContractResident",
                columns: table => new
                {
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidentType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractResident", x => new { x.ContractId, x.ResidentId });
                    table.ForeignKey(
                        name: "FK_ContractResident_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_ContractResident_Residents_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Residents",
                        principalColumn: "ResidentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Residents_ContractId",
                table: "Residents",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractResident_ResidentId",
                table: "ContractResident",
                column: "ResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Contracts_ContractId",
                table: "Residents",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Contracts_ContractId",
                table: "Residents");

            migrationBuilder.DropTable(
                name: "ContractResident");

            migrationBuilder.DropIndex(
                name: "IX_Residents_ContractId",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Residents");

            migrationBuilder.AddColumn<Guid>(
                name: "ApartmentId",
                table: "Residents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte>(
                name: "ResidentType",
                table: "Residents",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<Guid>(
                name: "ResidentId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Residents_ApartmentId",
                table: "Residents",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ResidentId",
                table: "Contracts",
                column: "ResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Residents_ResidentId",
                table: "Contracts",
                column: "ResidentId",
                principalTable: "Residents",
                principalColumn: "ResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Apartments_ApartmentId",
                table: "Residents",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "ApartmentId");
        }
    }
}
