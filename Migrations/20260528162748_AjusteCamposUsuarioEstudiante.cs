using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oficcios360.Migrations
{
    /// <inheritdoc />
    public partial class AjusteCamposUsuarioEstudiante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EstudianteId",
                schema: "public",
                table: "formaciones_academicas",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EstudianteId",
                schema: "public",
                table: "formaciones_academicas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
