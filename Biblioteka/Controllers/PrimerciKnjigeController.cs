using Biblioteka.Data;
using Biblioteka.Models;
using Biblioteka.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin,Bibliotekar")]
    public class PrimerciKnjigeController : AppController
    {
        public ApplicationDbContext _context;
        public PrimerciKnjigeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Lista(int? knjigaId)
        {
            ViewBag.knjigaId = knjigaId;
            return View(await _context.PrimerciKnjige.Where(p=>p.KnjigaId==knjigaId).Include(p=>p.Knjiga).ToListAsync());
        }

        [HttpGet("{knjigaId?}")]
        public async Task<IActionResult> Kreiraj(int ?knjigaId)
        {
            var knjige = _context.Knjige.ToListAsync();
            var primerakKnjigeVM = new PrimerakKnjigeVM(new PrimerakKnjige(),await knjige);
            if(knjigaId!=null)
            {
                primerakKnjigeVM.PrimerakKnjige.KnjigaId = (int)knjigaId;
            }
            return View(primerakKnjigeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KreirajPrimerak(PrimerakKnjigeVM primerakKnjigeVM)
        {
            var primerakKnjige = primerakKnjigeVM.PrimerakKnjige;
            List<PrimerakKnjige> primerciKnjige = new List<PrimerakKnjige>();
            for(int i=0;i<primerakKnjigeVM.BrojPrimerakaZaDodavanje;i++)
            {
                primerciKnjige.Add((PrimerakKnjige)_context.Entry(primerakKnjige).CurrentValues.ToObject());//napravi metodu za kloniranje modela
            }
            if (ModelState.IsValid)
            {
                _context.AddRange(primerciKnjige);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(KnjigeController.Prikazi),"Knjige",new { @id = primerakKnjige.KnjigaId });
            }
            return View(primerakKnjigeVM);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Izmeni(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var primerakKnjige = await _context.PrimerciKnjige.Include(p => p.Knjiga).Where(p => p.PrimerakKnjigeId == id).FirstOrDefaultAsync();
            var knjige = _context.Knjige.ToList();
            var knjigaVM = new PrimerakKnjigeVM(primerakKnjige,knjige);
            if (primerakKnjige == null)
            {
                return NotFound();
            }
            return View(knjigaVM);
        }
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmeni(int id, PrimerakKnjigeVM primerakKnjigeVM)
        {
            var primerakKnjige = primerakKnjigeVM.PrimerakKnjige;
            if (id != primerakKnjige.PrimerakKnjigeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(primerakKnjige);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrimerakPostoji(primerakKnjige.PrimerakKnjigeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(KnjigeController.Prikazi),"Knjige",new {@id=primerakKnjige.KnjigaId });
            }
            primerakKnjige = await _context.PrimerciKnjige.Include(p => p.Knjiga).Where(p => p.PrimerakKnjigeId == id).FirstOrDefaultAsync();
            var knjige = _context.Knjige.ToList();
            primerakKnjigeVM = new PrimerakKnjigeVM(primerakKnjige, knjige);
            return View(primerakKnjigeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            var primerakKnjige = await _context.PrimerciKnjige.Include(p=>p.Knjiga).FirstAsync(p=>p.PrimerakKnjigeId==id);
            _context.PrimerciKnjige.Remove(primerakKnjige);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(KnjigeController.Prikazi),"Knjige",new {@id=primerakKnjige.KnjigaId });
        }
        public async Task<IActionResult> IstorijaIznajmljivanja(int primerakID)
        {
            var primerak = await _context.PrimerciKnjige.
                Include(p => p.Iznajmljivanja).ThenInclude(i=>i.Korisnik).
                Include(p=>p.Iznajmljivanja).ThenInclude(i=>i.Bibliotekar).
                Include(p => p.Knjiga).FirstOrDefaultAsync(k => k.PrimerakKnjigeId == primerakID);
            return View(primerak);
        }
      

        private bool PrimerakPostoji(int id)
        {
            return _context.PrimerciKnjige.Any(e => e.PrimerakKnjigeId == id);
        }
    }
}
