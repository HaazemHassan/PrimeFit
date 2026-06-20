using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "payments");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "subscriptions",
                table: "Packages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "egp");

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                schema: "payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    StripePaymentIntentId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PaidAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.CheckConstraint("CK_PaymentTransaction_Amount_NonNegative", "[Amount] >= 0");
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_DomainUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "users",
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Packages_PackageId",
                        column: x => x.PackageId,
                        principalSchema: "subscriptions",
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_PackageId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_StripePaymentIntentId",
                schema: "payments",
                table: "PaymentTransactions",
                column: "StripePaymentIntentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_User_Status",
                schema: "payments",
                table: "PaymentTransactions",
                columns: new[] { "UserId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTransactions",
                schema: "payments");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "subscriptions",
                table: "Packages");
        }
    }
}
