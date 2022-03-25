using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteka.Controllers
{
    [Authorize(Roles = "Admin,Bibliotekar")]
    public class AutoriController : AppController
    {
        private readonly ApplicationDbContext _context;

        public AutoriController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Lista()
        {
            return View(await _context.Autori.ToListAsync());
        }

        public IActionResult Kreiraj()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kreiraj(Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            return View(autor);
        }

        public async Task<IActionResult> Izmeni(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autori.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmeni(int id, Autor autor)
        {
            if (id != autor.AutorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorPostoji(autor.AutorId))
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
            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            var autor = await _context.Autori.FindAsync(id);
            _context.Autori.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        private bool AutorPostoji(int id)
        {
            return _context.Autori.Any(e => e.AutorId == id);
        }
    }
}
