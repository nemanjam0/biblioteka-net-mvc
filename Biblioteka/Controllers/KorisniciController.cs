using Biblioteka.Data;
using Biblioteka.Enums;
using Biblioteka.Models;
using Biblioteka.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Controllers
{
    [Authorize(Roles = "Admin,Bibliotekar")]
    public class KorisniciController : AppController
    {
        private readonly ApplicationDbContext _context;
        public KorisniciController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Lista(PaginiranaListaVM<PretragaKorisnikaVM> vm)
        {
            //if (vm.Podaci is null) vm.Podaci = new PretragaKorisnikaVM();//znaci da je u pitanju pr

            var url = HttpContext.Request.GetEncodedUrl();
            const int velicinaStrane = 10;
            var query = _context.Korisnici.AsQueryable();
            if (vm.Podaci.Email is not null) query = query.Where(i => i.Email.Contains(vm.Podaci.Email));
            if (vm.Podaci.Ime is not null) query = query.Where(i => i.Ime.Contains(vm.Podaci.Ime));
            if (vm.Podaci.Prezime is not null) query = query.Where(i => i.Prezime.Contains(vm.Podaci.Prezime));
            if (vm.Podaci.Id is not null) query = query.Where(i => i.Id == vm.Podaci.Id);
            var korisnici = await query.Skip((vm.Podaci.Strana - 1) * velicinaStrane).Take(velicinaStrane).ToListAsync();
            vm.Podaci.Korisnici = korisnici;
            var PodaciVM = new PaginiranaListaVM<PretragaKorisnikaVM>(url, vm.Podaci.Strana, "Podaci.Strana", vm.Podaci);
            return View(PodaciVM);
        }
        [HttpGet]
        public async Task<IActionResult> ProduziClanskuKartu(int id)
        {
            var korisnik = await _context.Korisnici.
                Select(k => new ProduziClanskuKartuVM
                {
                    korisnikId = k.Id,
                    Ime = k.Ime,
                    Prezime=k.Prezime,
                    RokVazenja=k.VazenjeClanskeKarte
                }).FirstOrDefaultAsync(k=>k.korisnikId==id);
            return View(korisnik);
        }
        [HttpPost]
        public async Task<IActionResult> ProduziClanskuKartu(ProduziClanskuKartuVM produziVM)
        {
            if(ModelState.IsValid)
            {
                var korisnik = await _context.Korisnici.FindAsync(produziVM.korisnikId);
                if(korisnik is null)
                {
                    return NotFound();
                }
                korisnik.VazenjeClanskeKarte = produziVM.RokVazenja;
                _context.Update(korisnik);
                await _context.SaveChangesAsync();
            }
            return View(nameof(ProduziClanskuKartu), produziVM, MessageType.Success, "Članska karta uspešno produžena");
        }
    }
}
