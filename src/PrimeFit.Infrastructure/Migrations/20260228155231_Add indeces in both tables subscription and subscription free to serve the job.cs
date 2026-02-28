using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addindecesinbothtablessubscriptionandsubscriptionfreetoservethejob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Job_Processing",
                table: "Subscriptions",
                columns: new[] { "NextProcessingDate", "Id" },
                filter: "[NextProcessingDate] IS NOT NULL AND [Status] <> 3 AND [Status] <> 4");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_User_Branch_Status",
                table: "Subscriptions",
                columns: new[] { "UserId", "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionFreezes_ActiveFreezes",
                table: "SubscriptionFreezes",
                columns: new[] { "SubscriptionId", "EndDate" },
                filter: "[EndDate] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_Job_Processing",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_User_Branch_Status",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionFreezes_ActiveFreezes",
                table: "SubscriptionFreezes");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");
        }
    }
}
