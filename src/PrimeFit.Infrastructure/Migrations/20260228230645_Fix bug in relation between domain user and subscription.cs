using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fixbuginrelationbetweendomainuserandsubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_DomainUser_DomainUserId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_DomainUserId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DomainUserId",
                table: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomainUserId",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_DomainUserId",
                table: "Subscriptions",
                column: "DomainUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_DomainUser_DomainUserId",
                table: "Subscriptions",
                column: "DomainUserId",
                principalTable: "DomainUser",
                principalColumn: "Id");
        }
    }
}
