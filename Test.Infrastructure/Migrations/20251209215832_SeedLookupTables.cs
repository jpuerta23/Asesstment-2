using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Test.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedLookupTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete existing data from all lookup tables
            migrationBuilder.Sql("DELETE FROM \"Empleados\";");
            migrationBuilder.Sql("DELETE FROM \"Cargos\";");
            migrationBuilder.Sql("DELETE FROM \"Departamentos\";");
            migrationBuilder.Sql("DELETE FROM \"NivelesEducativos\";");
            migrationBuilder.Sql("DELETE FROM \"EstadosEmpleado\";");

            migrationBuilder.InsertData(
                table: "Cargos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Gerente" },
                    { 2, "Supervisor" },
                    { 3, "Analista" },
                    { 4, "Asistente" },
                    { 5, "Técnico" }
                });

            migrationBuilder.InsertData(
                table: "Departamentos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Recursos Humanos" },
                    { 2, "Tecnología" },
                    { 3, "Ventas" },
                    { 4, "Finanzas" },
                    { 5, "Operaciones" }
                });

            migrationBuilder.InsertData(
                table: "EstadosEmpleado",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Activo" },
                    { 2, "Inactivo" },
                    { 3, "Vacaciones" }
                });

            migrationBuilder.InsertData(
                table: "NivelesEducativos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Bachillerato" },
                    { 2, "Técnico" },
                    { 3, "Tecnólogo" },
                    { 4, "Profesional" },
                    { 5, "Especialización" },
                    { 6, "Maestría" },
                    { 7, "Doctorado" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EstadosEmpleado",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstadosEmpleado",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstadosEmpleado",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NivelesEducativos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NivelesEducativos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NivelesEducativos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NivelesEducativos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "NivelesEducativos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "NivelesEducativos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "NivelesEducativos",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
