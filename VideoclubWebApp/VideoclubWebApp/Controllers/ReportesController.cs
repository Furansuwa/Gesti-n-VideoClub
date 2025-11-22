using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VideoClubWebApp.Data;
using VideoclubWebApp.Services;
using VideoclubWebApp.Models.Articulos;


namespace VideoclubWebApp.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPdfService _pdfService;

        public ReportesController(ApplicationDbContext context, IPdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        // GET: Reportes
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reportes/ArticulosPdf
        public async Task<IActionResult> ArticulosPdf()
        {
            var articulos = await _context.Articulos.ToListAsync();
            var html = GenerarHtmlArticulos(articulos);
            var pdfBytes = _pdfService.GeneratePdf(html);

            return File(pdfBytes, "application/pdf", $"Articulos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        // GET: Reportes/ClientesPdf
        public async Task<IActionResult> ClientesPdf()
        {
            var clientes = await _context.Clientes.ToListAsync();
            var html = GenerarHtmlClientes(clientes);
            var pdfBytes = _pdfService.GeneratePdf(html);

            return File(pdfBytes, "application/pdf", $"Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        // GET: Reportes/EmpleadosPdf
        public async Task<IActionResult> EmpleadosPdf()
        {
            var empleados = await _context.Empleados.ToListAsync();
            var html = GenerarHtmlEmpleados(empleados);
            var pdfBytes = _pdfService.GeneratePdf(html);

            return File(pdfBytes, "application/pdf", $"Empleados_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        // GET: Reportes/RentasPdf
        public async Task<IActionResult> RentasPdf(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Rentas.AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(r => r.FechaRenta >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(r => r.FechaRenta <= fechaFin.Value);

            var rentas = await query.ToListAsync();
            var html = GenerarHtmlRentas(rentas, fechaInicio, fechaFin);
            var pdfBytes = _pdfService.GeneratePdf(html);

            return File(pdfBytes, "application/pdf", $"Rentas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        // Métodos privados para generar HTML

        private string GenerarHtmlArticulos(List<VideoclubWebApp.Models.Articulos.Articulo> articulos)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8' />
                    <title>Reporte de Artículos</title>
                    <style>
                        body { font-family: Arial, sans-serif; }
                        h1 { color: #333; text-align: center; }
                        table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                        th { background-color: #4CAF50; color: white; padding: 12px; text-align: left; }
                        td { padding: 8px; border-bottom: 1px solid #ddd; }
                        tr:hover { background-color: #f5f5f5; }
                        .total { font-weight: bold; margin-top: 20px; }
                    </style>
                </head>
                <body>
                    <h1>Reporte de Artículos</h1>
                    <p>Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</p>
                    <table>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Título</th>
                                <th>Renta por Día</th>
                                <th>Días Renta</th>
                                <th>Estado</th>
                            </tr>
                        </thead>
                        <tbody>");

            foreach (var articulo in articulos)
            {
                sb.Append($@"
                    <tr>
                        <td>{articulo.Id}</td>
                        <td>{articulo.Titulo}</td>
                        <td>${articulo.RentaPorDia:N2}</td>
                        <td>{articulo.DiasRenta}</td>
                        <td>{articulo.Estado}</td>
                    </tr>");
            }

            sb.Append($@"
                        </tbody>
                    </table>
                    <p class='total'>Total de artículos: {articulos.Count}</p>
                </body>
                </html>");

            return sb.ToString();
        }

        // ReportesController.cs - Método GenerarHtmlClientes corregido

        private string GenerarHtmlClientes(List<VideoClubWebApp.Models.Cliente> clientes)
        {
            var sb = new StringBuilder();
            sb.Append(@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='utf-8' />
            <title>Reporte de Clientes</title>
            <style>
                body { font-family: Arial, sans-serif; }
                h1 { color: #333; text-align: center; }
                table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                th { background-color: #2196F3; color: white; padding: 12px; text-align: left; }
                td { padding: 8px; border-bottom: 1px solid #ddd; }
                tr:hover { background-color: #f5f5f5; }
                .total { font-weight: bold; margin-top: 20px; }
            </style>
        </head>
        <body>
            <h1>Reporte de Clientes</h1>
            <p>Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</p>
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>Cédula</th>
                        <th>Tipo</th>
                        <th>Límite Crédito</th>
                        <th>Estado</th>
                    </tr>
                </thead>
                <tbody>");

            foreach (var cliente in clientes)
            {
                sb.Append($@"
            <tr>
                <td>{cliente.Id}</td>
                <td>{cliente.Nombre}</td>
                <td>{cliente.Cedula}</td>
                <td>{cliente.TipoPersona}</td>
                <td>${cliente.LimiteCredito:N2}</td>
                <td>{cliente.Estado}</td>
            </tr>");
            }

            sb.Append($@"
                </tbody>
            </table>
            <p class='total'>Total de clientes: {clientes.Count}</p>
        </body>
        </html>");

            return sb.ToString();
        }

        private string GenerarHtmlEmpleados(List<VideoClubWebApp.Models.Empleado> empleados)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8' />
                    <title>Reporte de Empleados</title>
                    <style>
                        body { font-family: Arial, sans-serif; }
                        h1 { color: #333; text-align: center; }
                        table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                        th { background-color: #FF9800; color: white; padding: 12px; text-align: left; }
                        td { padding: 8px; border-bottom: 1px solid #ddd; }
                        tr:hover { background-color: #f5f5f5; }
                        .total { font-weight: bold; margin-top: 20px; }
                    </style>
                </head>
                <body>
                    <h1>Reporte de Empleados</h1>
                    <p>Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</p>
                    <table>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Nombre</th>
                                <th>Cédula</th>
                                <th>Tanda</th>
                                <th>Comisión %</th>
                                <th>Fecha Ingreso</th>
                            </tr>
                        </thead>
                        <tbody>");

            foreach (var empleado in empleados)
            {
                sb.Append($@"
                    <tr>
                        <td>{empleado.Id}</td>
                        <td>{empleado.Nombre}</td>
                        <td>{empleado.Cedula}</td>
                        <td>{empleado.TandaLabor}</td>
                        <td>{empleado.PorcientoComision}%</td>
                        <td>{empleado.FechaIngreso:dd/MM/yyyy}</td>
                    </tr>");
            }

            sb.Append($@"
                        </tbody>
                    </table>
                    <p class='total'>Total de empleados: {empleados.Count}</p>
                </body>
                </html>");

            return sb.ToString();
        }

        private string GenerarHtmlRentas(List<VideoClubWebApp.Models.Rentas> rentas, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var sb = new StringBuilder();
            var totalRentas = rentas.Sum(r => r.MontoPorDia * r.CantidadDias);

            sb.Append(@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8' />
                    <title>Reporte de Rentas</title>
                    <style>
                        body { font-family: Arial, sans-serif; }
                        h1 { color: #333; text-align: center; }
                        table { width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 12px; }
                        th { background-color: #9C27B0; color: white; padding: 10px; text-align: left; }
                        td { padding: 6px; border-bottom: 1px solid #ddd; }
                        tr:hover { background-color: #f5f5f5; }
                        .total { font-weight: bold; margin-top: 20px; font-size: 16px; }
                        .filtros { background-color: #f0f0f0; padding: 10px; margin-bottom: 15px; }
                    </style>
                </head>
                <body>
                    <h1>Reporte de Rentas</h1>
                    <p>Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</p>");

            if (fechaInicio.HasValue || fechaFin.HasValue)
            {
                sb.Append(@"<div class='filtros'><strong>Filtros aplicados:</strong> ");
                if (fechaInicio.HasValue)
                    sb.Append($"Desde: {fechaInicio.Value:dd/MM/yyyy} ");
                if (fechaFin.HasValue)
                    sb.Append($"Hasta: {fechaFin.Value:dd/MM/yyyy}");
                sb.Append("</div>");
            }

            sb.Append(@"
                    <table>
                        <thead>
                            <tr>
                                <th>No. Renta</th>
                                <th>Fecha Renta</th>
                                <th>Fecha Devolución</th>
                                <th>Cliente ID</th>
                                <th>Artículo ID</th>
                                <th>Días</th>
                                <th>Monto/Día</th>
                                <th>Total</th>
                                <th>Estado</th>
                            </tr>
                        </thead>
                        <tbody>");

            foreach (var renta in rentas)
            {
                var total = renta.MontoPorDia * renta.CantidadDias;
                sb.Append($@"
                    <tr>
                        <td>{renta.NoRenta}</td>
                        <td>{renta.FechaRenta:dd/MM/yyyy}</td>
                        <td>{(renta.FechaDevolucion.HasValue ? renta.FechaDevolucion.Value.ToString("dd/MM/yyyy") : "Pendiente")}</td>
                        <td>{renta.ClienteId}</td>
                        <td>{renta.ArticuloId}</td>
                        <td>{renta.CantidadDias}</td>
                        <td>${renta.MontoPorDia:N2}</td>
                        <td>${total:N2}</td>
                        <td>{renta.Estado}</td>
                    </tr>");
            }

            sb.Append($@"
                        </tbody>
                    </table>
                    <p class='total'>Total de rentas: {rentas.Count}</p>
                    <p class='total'>Total recaudado: ${totalRentas:N2}</p>
                </body>
                </html>");

            return sb.ToString();
        }
    }
}