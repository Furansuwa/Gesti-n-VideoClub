using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoclubWebApp.Models;
using VideoclubWebApp.Models.Articulos;
using VideoclubWebApp.Models.Elenco;
using VideoClubWebApp.Data;

namespace VideoclubWebApp.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticulosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Articulos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articulos.ToListAsync());
        }

        // GET: Articulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.ElencoArticulos)
                    .ThenInclude(ea => ea.Elenco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (articulo == null)
            {
                return NotFound();
            }

            // Cargar información adicional
            ViewBag.TipoArticulo = await _context.TiposArticulos.FindAsync(articulo.TipoArticuloId);
            ViewBag.Genero = await _context.Generos.FindAsync(articulo.GeneroId);
            ViewBag.Idioma = await _context.Idiomas.FindAsync(articulo.IdiomaId);

            return View(articulo);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            var viewModel = new ArticuloElencoViewModel();

            // Cargar todos los elencos disponibles (solo activos)
            var todosLosElencos = _context.Elencos.Where(e => e.Estado == "Activo").ToList();

            foreach (var elenco in todosLosElencos)
            {
                viewModel.ElencosDisponibles.Add(new ElencoAsignadoViewModel
                {
                    ElencoId = elenco.Id,
                    Nombre = elenco.Nombre,
                    Asignado = false,
                    Rol = ""
                });
            }

            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion");
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion");
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion");
            return View(viewModel);
        }

        // POST: Articulos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticuloElencoViewModel viewModel)
        {
            // Validamos solo el modelo de Artículo
            if (ModelState.IsValid)
            {
                // 1. Guardar el artículo primero
                _context.Add(viewModel.Articulo);
                await _context.SaveChangesAsync(); // <-- Se genera el ID del Artículo

                // 2. Guardar las relaciones de elenco
                foreach (var el in viewModel.ElencosDisponibles.Where(e => e.Asignado))
                {
                    var elencoArticulo = new ElencoArticulo
                    {
                        ArticuloId = viewModel.Articulo.Id,
                        ElencoId = el.ElencoId,
                        Rol = el.Rol ?? "" // Asignar rol
                    };
                    _context.ElencoArticulos.Add(elencoArticulo);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Si falla, recargar los dropdowns y devolver el viewModel
            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion", viewModel.Articulo.IdiomaId);
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion", viewModel.Articulo.TipoArticuloId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion", viewModel.Articulo.GeneroId);
            return View(viewModel);
        }

        // GET: Articulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cargar el artículo Y sus relaciones de elenco existentes
            var articulo = await _context.Articulos
                .Include(a => a.ElencoArticulos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (articulo == null)
            {
                return NotFound();
            }

            var viewModel = new ArticuloElencoViewModel
            {
                Articulo = articulo
            };

            // Cargar TODOS los elencos activos
            var todosLosElencos = await _context.Elencos.Where(e => e.Estado == "Activo").ToListAsync();

            foreach (var elenco in todosLosElencos)
            {
                // Buscar si este elenco ya está asignado al artículo
                var elencoAsignado = articulo.ElencoArticulos.FirstOrDefault(ea => ea.ElencoId == elenco.Id);

                viewModel.ElencosDisponibles.Add(new ElencoAsignadoViewModel
                {
                    ElencoId = elenco.Id,
                    Nombre = elenco.Nombre,
                    Asignado = (elencoAsignado != null), // Marcar el checkbox si existe
                    Rol = (elencoAsignado != null) ? elencoAsignado.Rol : "" // Poner el rol si existe
                });
            }

            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion", articulo.IdiomaId);
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion", articulo.TipoArticuloId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion", articulo.GeneroId);
            return View(viewModel);
        }

        // POST: Articulos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticuloElencoViewModel viewModel)
        {
            if (id != viewModel.Articulo.Id)
            {
                return NotFound();
            }

            // Validamos solo el modelo de Artículo
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Actualizar el artículo
                    _context.Update(viewModel.Articulo);

                    // 2. Borrar elenco existente
                    var elencoExistente = _context.ElencoArticulos.Where(ea => ea.ArticuloId == id).ToList();
                    _context.ElencoArticulos.RemoveRange(elencoExistente);

                    // 3. Añadir el nuevo elenco (solo los asignados)
                    foreach (var el in viewModel.ElencosDisponibles.Where(e => e.Asignado))
                    {
                        var elencoArticulo = new ElencoArticulo
                        {
                            ArticuloId = id,
                            ElencoId = el.ElencoId,
                            Rol = el.Rol ?? ""
                        };
                        _context.ElencoArticulos.Add(elencoArticulo);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Articulos.Any(e => e.Id == viewModel.Articulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Si falla, recargar los dropdowns
            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion", viewModel.Articulo.IdiomaId);
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion", viewModel.Articulo.TipoArticuloId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion", viewModel.Articulo.GeneroId);
            return View(viewModel);
        }

        // GET: Articulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.ElencoArticulos)
                    .ThenInclude(ea => ea.Elenco)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (articulo == null)
            {
                return NotFound();
            }

            // Cargar información adicional
            ViewBag.TipoArticulo = await _context.TiposArticulos.FindAsync(articulo.TipoArticuloId);
            ViewBag.Genero = await _context.Generos.FindAsync(articulo.GeneroId);
            ViewBag.Idioma = await _context.Idiomas.FindAsync(articulo.IdiomaId);

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articulo = await _context.Articulos
                .Include(a => a.ElencoArticulos)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (articulo != null)
            {
                // Primero eliminar las relaciones con elenco
                _context.ElencoArticulos.RemoveRange(articulo.ElencoArticulos);

                // Luego eliminar el artículo
                _context.Articulos.Remove(articulo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.Id == id);
        }
    }
}