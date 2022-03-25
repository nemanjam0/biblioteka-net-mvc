using Biblioteka.Data;
using Biblioteka.Enums;
using Biblioteka.Models;
using Biblioteka.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteka.Controllers
{
    public class IznajmljivanjaController:AppController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;
        public IznajmljivanjaController(ApplicationDbContext context,UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public IActionResult Iznajmi()
        {
            return View(new Iznajmljivanje());
        }
       
        [HttpPost]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult> Iznajmi(Iznajmljivanje iznajmljivanje)
        {
            if(ModelState.IsValid)
            {
                iznajmljivanje.Korisnik = _context.Korisnici.Find(iznajmljivanje.KorisnikId);
                if(iznajmljivanje.Korisnik==null)
                {
                    ModelState.AddModelError("KorisnikID", "Korisnik sa tim brojem članske karte ne postoji.");
                }
                iznajmljivanje.Primerak = _context.PrimerciKnjige.Include(p=>p.Knjiga).FirstOrDefault(p=>p.PrimerakKnjigeId==iznajmljivanje.PrimerakId);
                if (iznajmljivanje.Primerak == null)
                {
                    ModelState.AddModelError("PrimerakID", "Knjiga sa tim brojem ne postoji.");
                }
                var knjigaJeVecIznajmljena = _context.Iznajmljivanja.Where(i => i.DatumVracanja == null).FirstOrDefault(i => i.PrimerakId == iznajmljivanje.PrimerakId);
                if (knjigaJeVecIznajmljena != null)
                {
                    ModelState.AddModelError("PrimerakID", "Taj primerak je već iznajmljen.");
                }
                if(iznajmljivanje.Korisnik?.VazenjeClanskeKarte is null || iznajmljivanje.Korisnik?.VazenjeClanskeKarte<DateTime.Now)
                {
                    ModelState.AddModelError("KorisnikID", "Članska karta je istekla.");
                }
                if (ModelState.IsValid)
                {
                    return View("IznajmiPotvrda",iznajmljivanje);
                    
                }
                return View();
        
            }
            return View();
        }
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult> IznajmljivanjePotvrdi(Iznajmljivanje iznajmljivanje)
        {
            if (ModelState.IsValid)
            {
                iznajmljivanje.VremeUzimanja = DateTime.Now;
                iznajmljivanje.BibliotekarId = int.Parse(_userManager.GetUserId(User));
                _context.Add(iznajmljivanje);
                await _context.SaveChangesAsync();
                return View("Iznajmi",iznajmljivanje,MessageType.Success,"Knjiga uspešno iznajmljena.");
            }
            return View(iznajmljivanje);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public IActionResult Vrati()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult>Vrati(VratiKnjiguVM vratiKnjiguVM)
        {
            //Console.WriteLine(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            if (ModelState.IsValid)
            {
                var primerakKnjige = await _context.PrimerciKnjige.
                    Include(p => p.Knjiga).
                    Include(p => p.Iznajmljivanja.Where(i => i.DatumVracanja == null)).
                    ThenInclude(i => i.Korisnik).
                    Include(p => p.Iznajmljivanja.Where(i => i.DatumVracanja == null)).
                    ThenInclude(i => i.Bibliotekar).
                    FirstOrDefaultAsync(p=>p.PrimerakKnjigeId==vratiKnjiguVM.primerakKnjigeID);
                if(primerakKnjige is null)
                {
                    ModelState.AddModelError("primerakKnjigeID", "Knjiga sa tim brojem ne postoji.");
                    return View();
                }
                if (primerakKnjige.Iznajmljivanja.Count==0)
                {
                    ModelState.AddModelError("primerakKnjigeID", "Knjiga se već nalazi u biblioteci.");
                    return View();
                }
                return View("VratiPotvrda", primerakKnjige.Iznajmljivanja.First());
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotekar")]
        public async Task<IActionResult> VratiPotvrda(int id,Iznajmljivanje iznajmljivanje)
        {
            {
                if (id != iznajmljivanje.IznajmljivanjeId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        iznajmljivanje.DatumVracanja = DateTime.Now;
                        iznajmljivanje.BibliotekarId = int.Parse(_userManager.GetUserId(User));
                        _context.Update(iznajmljivanje);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!IznajmljivanjePostoji(iznajmljivanje.IznajmljivanjeId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return View("Vrati", new VratiKnjiguVM(), MessageType.Success, "Knjiga uspešno vraćena.");
                }
                return View("Vrati", new VratiKnjiguVM(), MessageType.Alert, "Dogodila se greška");  
            }
        }
        public async Task<IActionResult> Moja()
        {
            string idKorisnika = _userManager.GetUserId(User);
            return RedirectToAction("ListaZaKorisnika",new{ id=idKorisnika});
        }
        [HttpGet]
        public async Task<IActionResult> ListaZaKorisnika([FromRoute]int id)
        {

            if(User.IsInRole("Korisnik") && id.ToString()!= _userManager.GetUserId(User))
            {
                return Forbid();
            }
            var primerci2 = await _context.Iznajmljivanja.Where(i => i.KorisnikId == id).
                Include(i => i.Primerak).ThenInclude(p => p.Knjiga).
                Select(i => new ListaPrimerakaKnjigeVM
                {
                    PrimerakKnjigaId = i.PrimerakId,
                    NazivKnjige = i.Primerak.Knjiga.Naziv,
                    IzdanjeKnjige = i.Primerak.Knjiga.Izdanje,
                    VremeNabavke = i.Primerak.VremeNabavke,
                    ImeKorisnika = i.Korisnik.Ime + " " + i.Korisnik.Prezime,
                    ImeBibliotekara = i.Bibliotekar.Ime + " " + i.Bibliotekar.Prezime,
                    VremeUzimanja = i.VremeUzimanja,
                    RokVracanja = i.RokVracanja,
                    DatumVracanja=i.DatumVracanja
                }).OrderByDescending(i => i.RokVracanja).ToListAsync();
            return View("ListaZaKorisnika", primerci2);
        }
        private bool IznajmljivanjePostoji(int id)
        {
            return _context.Iznajmljivanja.Any(e => e.IznajmljivanjeId == id);
        }

    }
}
