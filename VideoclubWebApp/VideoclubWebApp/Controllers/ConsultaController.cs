using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using VideoClubWebApp.Data;

namespace VideoclubWebApp.Controllers
{
    [Authorize(Roles = "Admin")] // Opcional: Restringe el acceso solo a administradores
    public class ConsultaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Consulta
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Consulta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                ViewData["ErrorMessage"] = "La consulta no puede estar vacía.";
                return View();
            }

            var model = new QueryResultViewModel();
            model.Query = query;

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Obtener los nombres de las columnas
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            model.ColumnHeaders.Add(reader.GetName(i));
                        }

                        // Obtener los datos de las filas
                        while (await reader.ReadAsync())
                        {
                            var row = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetValue(i));
                            }
                            model.Rows.Add(row);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                // Capturar y mostrar errores de SQL
                ViewData["ErrorMessage"] = $"Error al ejecutar la consulta: {ex.Message}";
            }
            finally
            {
                await connection.CloseAsync();
            }

            return View(model);
        }
    }

    // ViewModel para pasar los datos a la vista
    public class QueryResultViewModel
    {
        public string Query { get; set; } = "";
        public List<string> ColumnHeaders { get; set; } = new List<string>();
        public List<List<object>> Rows { get; set; } = new List<List<object>>();
    }
}