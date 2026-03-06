using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addspatialindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            """
            CREATE SPATIAL INDEX IX_Branches_Coordinates
            ON Branches(Coordinates)
            USING GEOGRAPHY_AUTO_GRID
            """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    DROP INDEX IX_Branches_Coordinates ON Branches
                    """
                );

        }
    }
}
