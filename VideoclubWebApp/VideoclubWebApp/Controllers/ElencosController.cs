using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoClubWebApp.Data;
using VideoclubWebApp.Models.Elenco;

namespace VideoclubWebApp.Controllers
{
    public class ElencosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElencosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Elencos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Elencos.ToListAsync());
        }

        // GET: Elencos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elenco = await _context.Elencos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (elenco == null)
            {
                return NotFound();
            }

            return View(elenco);
        }

        // GET: Elencos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Elencos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Estado")] Elenco elenco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(elenco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(elenco);
        }

        // GET: Elencos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elenco = await _context.Elencos.FindAsync(id);
            if (elenco == null)
            {
                return NotFound();
            }
            return View(elenco);
        }

        // POST: Elencos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Estado")] Elenco elenco)
        {
            if (id != elenco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(elenco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElencoExists(elenco.Id))
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
            return View(elenco);
        }

        // GET: Elencos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elenco = await _context.Elencos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (elenco == null)
            {
                return NotFound();
            }

            return View(elenco);
        }

        // POST: Elencos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var elenco = await _context.Elencos.FindAsync(id);
            if (elenco != null)
            {
                _context.Elencos.Remove(elenco);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElencoExists(int id)
        {
            return _context.Elencos.Any(e => e.Id == id);
        }
    }
}
