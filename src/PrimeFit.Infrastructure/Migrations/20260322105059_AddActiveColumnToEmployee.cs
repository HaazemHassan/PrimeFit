using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveColumnToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Employees");
        }
    }
}
