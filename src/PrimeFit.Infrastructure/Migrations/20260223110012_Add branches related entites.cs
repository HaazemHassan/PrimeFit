using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addbranchesrelatedentites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DurationInMonths = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    NumberOfFreezes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FreezeDurationInDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.CheckConstraint("CK_Package_DurationInMonths_Positive", "[DurationInMonths] > 0");
                    table.CheckConstraint("CK_Package_FreezeDurationInDays_NonNegative", "[FreezeDurationInDays] >= 0");
                    table.CheckConstraint("CK_Package_NumberOfFreezes_NonNegative", "[NumberOfFreezes] >= 0");
                    table.CheckConstraint("CK_Package_Price_Positive", "[Price] >= 0");
                    table.ForeignKey(
                        name: "FK_Package_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AllowedFreezeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AllowedFreezeDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DurationInMonths = table.Column<int>(type: "int", nullable: false),
                    DomainUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.CheckConstraint("CK_Subscription_AllowedFreezeCount_NonNegative", "[AllowedFreezeCount] >= 0");
                    table.CheckConstraint("CK_Subscription_AllowedFreezeDays_NonNegative", "[AllowedFreezeDays] >= 0");
                    table.CheckConstraint("CK_Subscription_DurationInMonths_Positive", "[DurationInMonths] > 0");
                    table.CheckConstraint("CK_Subscription_PaidAmount_NonNegative", "[PaidAmount] >= 0");
                    table.ForeignKey(
                        name: "FK_Subscription_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscription_DomainUser_DomainUserId",
                        column: x => x.DomainUserId,
                        principalTable: "DomainUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subscription_DomainUser_UserId",
                        column: x => x.UserId,
                        principalTable: "DomainUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscription_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionFreeze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxDays = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionFreeze", x => x.Id);
                    table.CheckConstraint("CK_SubscriptionFreeze_MaxDays_Positive", "[MaxDays] > 0");
                    table.ForeignKey(
                        name: "FK_SubscriptionFreeze_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Package_BranchId",
                table: "Package",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_BranchId",
                table: "Subscription",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_DomainUserId",
                table: "Subscription",
                column: "DomainUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_PackageId",
                table: "Subscription",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_UserId",
                table: "Subscription",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionFreeze_SubscriptionId",
                table: "SubscriptionFreeze",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionFreeze");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Package");
        }
    }
}
