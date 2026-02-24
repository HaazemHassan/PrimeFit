using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixbuginrealtionbetweenbranchandbranchImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchImages_Branches_BranchId1",
                table: "BranchImages");

            migrationBuilder.DropForeignKey(
                name: "FK_BranchImages_Branches_BranchId2",
                table: "BranchImages");

            migrationBuilder.DropIndex(
                name: "IX_BranchImages_BranchId1",
                table: "BranchImages");

            migrationBuilder.DropIndex(
                name: "IX_BranchImages_BranchId2",
                table: "BranchImages");

            migrationBuilder.DropColumn(
                name: "BranchId1",
                table: "BranchImages");

            migrationBuilder.DropColumn(
                name: "BranchId2",
                table: "BranchImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId1",
                table: "BranchImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId2",
                table: "BranchImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchImages_BranchId1",
                table: "BranchImages",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_BranchImages_BranchId2",
                table: "BranchImages",
                column: "BranchId2");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchImages_Branches_BranchId1",
                table: "BranchImages",
                column: "BranchId1",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchImages_Branches_BranchId2",
                table: "BranchImages",
                column: "BranchId2",
                principalTable: "Branches",
                principalColumn: "Id");
        }
    }
}
