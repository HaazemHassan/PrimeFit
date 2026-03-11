using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdduserTypepropertytotheuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "DomainUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "DomainUser");
        }
    }
}
