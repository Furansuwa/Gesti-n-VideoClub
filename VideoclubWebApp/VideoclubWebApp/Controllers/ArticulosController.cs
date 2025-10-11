using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoClubWebApp.Data;
using VideoClubWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion");
            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion");
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion");
            return View();
        }

        // POST: Articulos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,TipoArticuloId,IdiomaId,GeneroId,RentaPorDia,DiasRenta,MontoEntregaTardia,Estado")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion", articulo.TipoArticuloId);
            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion", articulo.IdiomaId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion", articulo.GeneroId);
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion", articulo.TipoArticuloId);
            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion", articulo.IdiomaId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion", articulo.GeneroId);
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,TipoArticuloId,IdiomaId,GeneroId,RentaPorDia,DiasRenta,MontoEntregaTardia,Estado")] Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.Id))
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
            ViewData["TipoArticuloId"] = new SelectList(_context.TiposArticulos, "Id", "Descripcion", articulo.TipoArticuloId);
            ViewData["IdiomaId"] = new SelectList(_context.Idiomas, "Id", "Descripcion", articulo.IdiomaId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Descripcion", articulo.GeneroId);
            return View(articulo);
        }

        // GET: Articulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.Id == id);
        }
    }
}