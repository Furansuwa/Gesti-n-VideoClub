using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoClubWebApp.Data;
using VideoClubWebApp.Models;
using VideoclubWebApp.Models.Articulos;

namespace VideoclubWebApp.Controllers
{
    [Authorize]
    public class RentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rentas
        public async Task<IActionResult> Index()
        {
            var rentas = await _context.Rentas.ToListAsync();
            return View(rentas);
        }

        // GET: Rentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renta = await _context.Rentas
                .FirstOrDefaultAsync(m => m.NoRenta == id);

            if (renta == null)
            {
                return NotFound();
            }

            // Cargar información relacionada
            ViewBag.Cliente = await _context.Clientes.FindAsync(renta.ClienteId);
            ViewBag.Articulo = await _context.Articulos.FindAsync(renta.ArticuloId);
            ViewBag.Empleado = await _context.Empleados.FindAsync(renta.EmpleadoId);

            return View(renta);
        }

        // GET: Rentas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes.Where(c => c.Estado == "Activo"), "Id", "Nombre");
            ViewData["ArticuloId"] = new SelectList(_context.Articulos.Where(a => a.Estado == "Disponible"), "Id", "Titulo");
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados.Where(e => e.Estado == "Activo"), "Id", "Nombre");
            return View();
        }

        // POST: Rentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,ArticuloId,EmpleadoId,CantidadDias,Comentario")] Rentas renta)
        {
            if (ModelState.IsValid)
            {
                // Obtener el artículo para calcular el monto
                var articulo = await _context.Articulos.FindAsync(renta.ArticuloId);

                if (articulo == null)
                {
                    ModelState.AddModelError("", "Artículo no encontrado");
                    return View(renta);
                }

                // Generar número de renta único
                var ultimaRenta = await _context.Rentas.OrderByDescending(r => r.NoRenta).FirstOrDefaultAsync();
                renta.NoRenta = (ultimaRenta?.NoRenta ?? 0) + 1;

                // Establecer valores
                renta.FechaRenta = DateTime.Now;
                renta.MontoPorDia = articulo.RentaPorDia;
                renta.Estado = "Activa";
                renta.FechaDevolucion = null;

                // Si no se especifica cantidad de días, usar los días predeterminados del artículo
                if (renta.CantidadDias <= 0)
                {
                    renta.CantidadDias = articulo.DiasRenta;
                }

                // Actualizar estado del artículo
                articulo.Estado = "Rentado";
                _context.Update(articulo);

                _context.Add(renta);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Renta #{renta.NoRenta} creada exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteId"] = new SelectList(_context.Clientes.Where(c => c.Estado == "Activo"), "Id", "Nombre", renta.ClienteId);
            ViewData["ArticuloId"] = new SelectList(_context.Articulos.Where(a => a.Estado == "Disponible"), "Id", "Titulo", renta.ArticuloId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados.Where(e => e.Estado == "Activo"), "Id", "Nombre", renta.EmpleadoId);
            return View(renta);
        }

        // GET: Rentas/Devolucion
        public async Task<IActionResult> Devolucion()
        {
            var rentasActivas = await _context.Rentas
                .Where(r => r.Estado == "Activa")
                .ToListAsync();

            return View(rentasActivas);
        }

        // GET: Rentas/ProcesarDevolucion/5
        public async Task<IActionResult> ProcesarDevolucion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renta = await _context.Rentas.FindAsync(id);

            if (renta == null || renta.Estado != "Activa")
            {
                return NotFound();
            }

            // Cargar información relacionada
            ViewBag.Cliente = await _context.Clientes.FindAsync(renta.ClienteId);
            ViewBag.Articulo = await _context.Articulos.FindAsync(renta.ArticuloId);
            ViewBag.Empleado = await _context.Empleados.FindAsync(renta.EmpleadoId);

            // Calcular días de retraso
            var fechaEsperada = renta.FechaRenta.AddDays(renta.CantidadDias);
            var diasRetraso = (DateTime.Now - fechaEsperada).Days;

            ViewBag.FechaEsperada = fechaEsperada;
            ViewBag.DiasRetraso = diasRetraso > 0 ? diasRetraso : 0;

            // Calcular montos
            var montoBase = renta.MontoPorDia * renta.CantidadDias;
            var articulo = await _context.Articulos.FindAsync(renta.ArticuloId);
            var montoRetraso = diasRetraso > 0 ? diasRetraso * articulo.MontoEntregaTardia : 0;
            var montoTotal = montoBase + montoRetraso;

            ViewBag.MontoBase = montoBase;
            ViewBag.MontoRetraso = montoRetraso;
            ViewBag.MontoTotal = montoTotal;

            return View(renta);
        }

        // POST: Rentas/ProcesarDevolucion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcesarDevolucionConfirmed(int id, string comentarioDevolucion)
        {
            var renta = await _context.Rentas.FindAsync(id);

            if (renta == null || renta.Estado != "Activa")
            {
                return NotFound();
            }

            // Actualizar la renta
            renta.FechaDevolucion = DateTime.Now;
            renta.Estado = "Devuelta";

            if (!string.IsNullOrEmpty(comentarioDevolucion))
            {
                renta.Comentario = string.IsNullOrEmpty(renta.Comentario)
                    ? comentarioDevolucion
                    : renta.Comentario + " | Devolución: " + comentarioDevolucion;
            }

            // Actualizar estado del artículo
            var articulo = await _context.Articulos.FindAsync(renta.ArticuloId);
            if (articulo != null)
            {
                articulo.Estado = "Disponible";
                _context.Update(articulo);
            }

            _context.Update(renta);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Devolución de renta #{renta.NoRenta} procesada exitosamente";
            return RedirectToAction(nameof(Devolucion));
        }

        // GET: Rentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renta = await _context.Rentas
                .FirstOrDefaultAsync(m => m.NoRenta == id);

            if (renta == null)
            {
                return NotFound();
            }

            // Cargar información relacionada
            ViewBag.Cliente = await _context.Clientes.FindAsync(renta.ClienteId);
            ViewBag.Articulo = await _context.Articulos.FindAsync(renta.ArticuloId);
            ViewBag.Empleado = await _context.Empleados.FindAsync(renta.EmpleadoId);

            return View(renta);
        }

        // POST: Rentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var renta = await _context.Rentas.FindAsync(id);

            if (renta != null)
            {
                // Si la renta está activa, devolver el artículo a disponible
                if (renta.Estado == "Activa")
                {
                    var articulo = await _context.Articulos.FindAsync(renta.ArticuloId);
                    if (articulo != null)
                    {
                        articulo.Estado = "Disponible";
                        _context.Update(articulo);
                    }
                }

                _context.Rentas.Remove(renta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RentaExists(int id)
        {
            return _context.Rentas.Any(e => e.NoRenta == id);
        }
    }
}