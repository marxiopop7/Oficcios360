using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Oficcios360.Migrations
{
    /// <inheritdoc />
    public partial class ModuloSeguimientoExplicito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "informes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PeriodoInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodoFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RutaFirmaTutor = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    EstudianteIdentificacion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_informes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "registro_actividades",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    HorasDedicadas = table.Column<double>(type: "double precision", nullable: false),
                    EstudianteIdentificacion = table.Column<string>(type: "text", nullable: false),
                    InformeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registro_actividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_registro_actividades_informes_InformeId",
                        column: x => x.InformeId,
                        principalSchema: "public",
                        principalTable: "informes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_registro_actividades_InformeId",
                schema: "public",
                table: "registro_actividades",
                column: "InformeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "registro_actividades",
                schema: "public");

            migrationBuilder.DropTable(
                name: "informes",
                schema: "public");
        }
    }
}
