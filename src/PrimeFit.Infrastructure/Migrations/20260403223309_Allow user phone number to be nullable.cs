using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Allowuserphonenumbertobenullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DomainUser_PhoneNumber",
                table: "DomainUser");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "DomainUser",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_DomainUser_PhoneNumber",
                table: "DomainUser",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DomainUser_PhoneNumber",
                table: "DomainUser");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "DomainUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainUser_PhoneNumber",
                table: "DomainUser",
                column: "PhoneNumber",
                unique: true);
        }
    }
}
