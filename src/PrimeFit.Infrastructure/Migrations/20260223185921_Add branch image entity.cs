using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addbranchimageentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_DomainUser_OwnerId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Package_Branches_BranchId",
                table: "Package");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Branches_BranchId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_DomainUser_DomainUserId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_DomainUser_UserId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Package_PackageId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionFreeze_Subscription_SubscriptionId",
                table: "SubscriptionFreeze");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionFreeze",
                table: "SubscriptionFreeze");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Package",
                table: "Package");

            migrationBuilder.RenameTable(
                name: "SubscriptionFreeze",
                newName: "SubscriptionFreezes");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Package",
                newName: "Packages");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionFreeze_SubscriptionId",
                table: "SubscriptionFreezes",
                newName: "IX_SubscriptionFreezes_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_UserId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_PackageId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_DomainUserId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_DomainUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_BranchId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Package_BranchId",
                table: "Packages",
                newName: "IX_Packages_BranchId");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Branches",
                type: "geography",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionFreezes",
                table: "SubscriptionFreezes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BranchImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    BranchId1 = table.Column<int>(type: "int", nullable: true),
                    BranchId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchImages_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchImages_Branches_BranchId1",
                        column: x => x.BranchId1,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BranchImages_Branches_BranchId2",
                        column: x => x.BranchId2,
                        principalTable: "Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchImages_BranchId",
                table: "BranchImages",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchImages_BranchId1",
                table: "BranchImages",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_BranchImages_BranchId2",
                table: "BranchImages",
                column: "BranchId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_DomainUser_OwnerId",
                table: "Branches",
                column: "OwnerId",
                principalTable: "DomainUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Branches_BranchId",
                table: "Packages",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionFreezes_Subscriptions_SubscriptionId",
                table: "SubscriptionFreezes",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Branches_BranchId",
                table: "Subscriptions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_DomainUser_DomainUserId",
                table: "Subscriptions",
                column: "DomainUserId",
                principalTable: "DomainUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_DomainUser_UserId",
                table: "Subscriptions",
                column: "UserId",
                principalTable: "DomainUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Packages_PackageId",
                table: "Subscriptions",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_DomainUser_OwnerId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Branches_BranchId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionFreezes_Subscriptions_SubscriptionId",
                table: "SubscriptionFreezes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Branches_BranchId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_DomainUser_DomainUserId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_DomainUser_UserId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Packages_PackageId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "BranchImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionFreezes",
                table: "SubscriptionFreezes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Branches");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameTable(
                name: "SubscriptionFreezes",
                newName: "SubscriptionFreeze");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "Package");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscription",
                newName: "IX_Subscription_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_PackageId",
                table: "Subscription",
                newName: "IX_Subscription_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_DomainUserId",
                table: "Subscription",
                newName: "IX_Subscription_DomainUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_BranchId",
                table: "Subscription",
                newName: "IX_Subscription_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionFreezes_SubscriptionId",
                table: "SubscriptionFreeze",
                newName: "IX_SubscriptionFreeze_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_BranchId",
                table: "Package",
                newName: "IX_Package_BranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionFreeze",
                table: "SubscriptionFreeze",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Package",
                table: "Package",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_DomainUser_OwnerId",
                table: "Branches",
                column: "OwnerId",
                principalTable: "DomainUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Branches_BranchId",
                table: "Package",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Branches_BranchId",
                table: "Subscription",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_DomainUser_DomainUserId",
                table: "Subscription",
                column: "DomainUserId",
                principalTable: "DomainUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_DomainUser_UserId",
                table: "Subscription",
                column: "UserId",
                principalTable: "DomainUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Package_PackageId",
                table: "Subscription",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionFreeze_Subscription_SubscriptionId",
                table: "SubscriptionFreeze",
                column: "SubscriptionId",
                principalTable: "Subscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
