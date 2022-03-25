using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class PretragaKorisnikaVM
    {
        [Display(Name = "Broj članske karte")]
        public int? Id { get; set; }
        [Display(Name = "Ime")]
        public string Ime { get; set; }

        [Display(Name = "Prezime")]
        public string Prezime { get; set; }
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Display(Name = "Strana")]
        public int Strana { get; set; } = 1;

        public IEnumerable<Korisnik> Korisnici { get; set; }
        public PretragaKorisnikaVM()
        {

        }
    }
}
