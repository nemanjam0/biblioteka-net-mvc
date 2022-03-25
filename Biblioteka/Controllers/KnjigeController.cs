using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;
using Biblioteka.Models;
using Biblioteka.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteka.Controllers
{
    public class KnjigeController : AppController
    {
        private readonly ApplicationDbContext _context;
        public UserManager<Korisnik> _userManager;

        public KnjigeController(ApplicationDbContext context,UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Lista(PaginiranaListaVM<PretragaKnjigaVM> viewModel)
        {
            var url = HttpContext.Request.GetEncodedUrl();
            const int velicinaStrane = 10;
            var query = _context.Knjige.Include(k => k.Autori).Include(k => k.Jezik).AsQueryable();
            if (viewModel.Podaci.Naziv is not null) query = query.Where(i => i.Naziv.Contains(viewModel.Podaci.Naziv));
            if (viewModel.Podaci.ImeAutora is not null) query = query.Include(k => k.Autori).Where(k=>k.Autori.Any(a => (a.Ime + a.Prezime).ToLower().Contains(viewModel.Podaci.ImeAutora)));
            if (viewModel.Podaci.JezikId is not null) query = query.Where(k => k.JezikId == viewModel.Podaci.JezikId);
            if (viewModel.Podaci.Izdanje is not null) query = query.Where(k => k.Izdanje == viewModel.Podaci.Izdanje);
            var korisnikId = _userManager.GetUserId(User);
            var knjigeVM = await query.Select(k =>
                    new KnjigaUListiVM
                    {
                        KnjigaId = k.KnjigaId,
                        NazivKnjige = k.Naziv,
                        IzdanjeKnjige = k.Izdanje,
                        GodinaIzdanja = k.GodinaIzdanja,
                        BrojStrana = k.BrojStrana,
                        NazivJezika = k.Jezik.Naziv,
                        ISBN = k.ISBN,
                        BrojPrimeraka = k.PrimerciKnjige.Count,
                        BrojIznajmljenihPrimeraka = k.PrimerciKnjige.Count(p=>(p.Iznajmljivanja.First(i => i.DatumVracanja == null) != null)),
                        AutoriKnjige = k.Autori,
                    }
                ).
                Skip((viewModel.Podaci.Strana - 1) * velicinaStrane).Take(velicinaStrane).ToListAsync(); ;
            viewModel.Podaci.Knjige = knjigeVM;
            viewModel.Podaci.Jezici = await _context.Jezici.ToListAsync();
            viewModel.PodesiPaginator(url, viewModel.Podaci.Strana, "Podaci.Strana");
            return View(viewModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Prikazi(int id)
        {
            var knjiga = await _context.Knjige.
                Include(k => k.PrimerciKnjige).
                ThenInclude(p => p.Iznajmljivanja.Where(i => i.DatumVracanja == null)).
                Include(k => k.Autori).
                Include(k => k.Jezik).FirstOrDefaultAsync(k=>k.KnjigaId==id);
            return View(knjiga);
        }
        [Authorize(Roles ="Admin,Bibliotekar")]
        public IActionResult Kreiraj()
        {
            var autori = _context.Autori.ToList();
            var jezici = _context.Jezici.ToList();
            var knjigaVM = new KreirajKnjiguVM(new Knjiga(),autori,jezici);
            return View(knjigaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult> Kreiraj(KreirajKnjiguVM KnjigaVM)
        {
            var knjiga = KnjigaVM.Knjiga;
            knjiga.Autori = new List<Autor>();
            foreach(var autorID in KnjigaVM.izabraniAutoriId)
            {
                Autor autor = new Autor() { AutorId = autorID };
                _context.Autori.Attach(autor);
                knjiga.Autori.Add(autor);
            }
            if (ModelState.IsValid)
            {
                _context.Add(KnjigaVM.Knjiga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            return View(KnjigaVM.Knjiga);
        }
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult> Izmeni(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige.Include(k=>k.Autori).Include(k=>k.Jezik).Include(k=>k.Autori).Where(k=>k.KnjigaId==id).FirstOrDefaultAsync();
            var autori = _context.Autori.ToList();
            var jezici = _context.Jezici.ToList();
            var knjigaVM = new KreirajKnjiguVM(knjiga, autori, jezici);
            if (knjiga == null)
            {
                return NotFound();
            }
            return View(knjigaVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult> Izmeni(int id, KreirajKnjiguVM knjigaVM)
        {
            var knjiga = knjigaVM.Knjiga;
            var knjigaIzBaze = await _context.Knjige.Include(k=>k.Autori).FirstOrDefaultAsync(k=>k.KnjigaId==id);
            var autori = await _context.Autori.Where(a => knjigaVM.izabraniAutoriId.Contains(a.AutorId)).ToListAsync();
            knjigaIzBaze.Autori = autori; knjigaIzBaze.ISBN = knjiga.ISBN; knjigaIzBaze.Naziv = knjiga.Naziv; knjigaIzBaze.BrojStrana = knjiga.BrojStrana; knjigaIzBaze.Opis = knjiga.Opis;knjigaIzBaze.JezikId = knjiga.JezikId; knjigaIzBaze.GodinaIzdanja = knjigaIzBaze.GodinaIzdanja;
            if (id != knjiga.KnjigaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(knjigaIzBaze);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnjigaPostoji(knjiga.KnjigaId))
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
            return View(knjiga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult> Obrisi(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);
            _context.Knjige.Remove(knjiga);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        private bool KnjigaPostoji(int id)
        {
            return _context.Knjige.Any(e => e.KnjigaId == id);
        }
    }
}
