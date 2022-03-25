using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class Korisnik:IdentityUser<int>
    {
        
        public string Ime { get; set; }
        public string Prezime { get; set; }
        [Display(Name ="Važenje članske karte")]
        public DateTime ?VazenjeClanskeKarte { get; set; }
        public ICollection<Iznajmljivanje> Iznajmljivanja { get; set; }//knjige koje je korisnik do sada uzeo iz biblioteke
        public ICollection<Iznajmljivanje> Izdavanja { get; set; }//knjige je je bibliotekar do sada izdao drugim korisnicima
    }
}
