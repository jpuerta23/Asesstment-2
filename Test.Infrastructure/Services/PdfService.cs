using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Test.Application.Interfaces;
using Test.Domain.Entities;

namespace Test.Infrastructure.Services;

public class PdfService : IPdfService
{
    public byte[] GenerateResume(Empleado empleado)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text($"Hoja de Vida: {empleado.Nombres} {empleado.Apellidos}")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Text("Datos Personales").Bold().FontSize(16);
                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Documento/ID:");
                            table.Cell().Text(empleado.Id.ToString());

                            table.Cell().Text("Fecha Nacimiento:");
                            table.Cell().Text(empleado.FechaNacimiento.ToShortDateString());

                            table.Cell().Text("Dirección:");
                            table.Cell().Text(empleado.Direccion);
                        });

                        x.Item().Text("Datos de Contacto").Bold().FontSize(16);
                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Teléfono:");
                            table.Cell().Text(empleado.Telefono);

                            table.Cell().Text("Email:");
                            table.Cell().Text(empleado.Email);
                        });

                        x.Item().Text("Información Laboral").Bold().FontSize(16);
                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Cargo:");
                            table.Cell().Text(empleado.Cargo?.Nombre ?? "N/A");

                            table.Cell().Text("Departamento:");
                            table.Cell().Text(empleado.Departamento?.Nombre ?? "N/A");

                            table.Cell().Text("Salario:");
                            table.Cell().Text($"{empleado.Salario:C}");

                            table.Cell().Text("Fecha Ingreso:");
                            table.Cell().Text(empleado.FechaIngreso.ToShortDateString());

                            table.Cell().Text("Estado:");
                            table.Cell().Text(empleado.EstadoEmpleado?.Nombre ?? "N/A");
                        });

                        x.Item().Text("Formación y Perfil").Bold().FontSize(16);
                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Nivel Educativo:");
                            table.Cell().Text(empleado.NivelEducativo?.Nombre ?? "N/A");

                            table.Cell().Text("Perfil Profesional:");
                            table.Cell().Text(empleado.PerfilProfesional);
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        })
        .GeneratePdf();
    }
}
