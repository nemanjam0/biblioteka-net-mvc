using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class Iznajmljivanje
    {
        public int IznajmljivanjeId { get; set; }
        [Display(Name = "Broj članske karte")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
    
        public PrimerakKnjige Primerak { get; set; }
        [Display(Name ="Broj knjige")]
        public int PrimerakId { get; set; }
        public int BibliotekarId { get; set; }
        public Korisnik Bibliotekar { get; set; }
        [Display(Name = "Vreme uzimanja")]
        public DateTime VremeUzimanja { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name ="Rok vraćanja")]
        public DateTime RokVracanja { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name ="Datum vraćanja")]
        public DateTime? DatumVracanja { get; set; }
    }

}
