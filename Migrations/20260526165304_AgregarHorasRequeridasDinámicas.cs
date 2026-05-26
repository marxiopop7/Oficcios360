using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oficcios360.Migrations
{
    /// <inheritdoc />
    public partial class AgregarHorasRequeridasDinámicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HorasRequeridas",
                schema: "public",
                table: "informes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorasRequeridas",
                schema: "public",
                table: "informes");
        }
    }
}
