using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Controllers
{
    [Authorize(Roles="Admin")]
    public class JeziciController : AppController
    {
        // GET: JeziciController
        private readonly ApplicationDbContext _context;
        public JeziciController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Lista()
        {
            return View(await _context.Jezici.ToListAsync());
        }

        public IActionResult Kreiraj()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kreiraj(Jezik jezik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jezik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            return View(jezik);
        }

        public async Task<IActionResult> Izmeni(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Jezici.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmeni(int id, Jezik jezik)
        {
            if (id != jezik.JezikId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jezik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JezikPostoji(jezik.JezikId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Lista));
            }
            return View(jezik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            var jezik = await _context.Jezici.FindAsync(id);
            _context.Jezici.Remove(jezik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        private bool JezikPostoji(int id)
        {
            return _context.Jezici.Any(e => e.JezikId == id);
        }
    }
}
