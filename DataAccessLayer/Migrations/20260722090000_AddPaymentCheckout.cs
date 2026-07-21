using DataAccessLayer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(CondoContext))]
    [Migration("20260722090000_AddPaymentCheckout")]
    public partial class AddPaymentCheckout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApartmentId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ApartmentId",
                table: "Bookings",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Apartments_ApartmentId",
                table: "Bookings",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "ApartmentId");

            // Preserve the meaning of legacy values before changing the enum layout:
            // Paid (1) -> Paid (3), LatePayment (2) -> Paid (3), Overdue (3) -> Overdue (4).
            migrationBuilder.Sql("UPDATE Payments SET PaymentStatus = CASE WHEN PaymentStatus IN (1, 2) THEN 3 WHEN PaymentStatus = 3 THEN 4 ELSE PaymentStatus END");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte>(
                name: "PaymentType",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCode",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.Sql("UPDATE Payments SET PaymentType = 1 WHERE BookingId IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Bookings_Apartments_ApartmentId", table: "Bookings");
            migrationBuilder.DropIndex(name: "IX_Bookings_ApartmentId", table: "Bookings");
            migrationBuilder.DropColumn(name: "ApartmentId", table: "Bookings");

            migrationBuilder.DropColumn(name: "PaymentType", table: "Payments");
            migrationBuilder.DropColumn(name: "TransactionId", table: "Payments");
            migrationBuilder.DropColumn(name: "ReferenceCode", table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.Sql("UPDATE Payments SET PaymentStatus = CASE WHEN PaymentStatus = 3 THEN 1 WHEN PaymentStatus = 4 THEN 3 ELSE 0 END");
        }
    }
}
