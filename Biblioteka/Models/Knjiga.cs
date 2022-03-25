using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class Knjiga
    {
        public int KnjigaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public int Izdanje { get; set; }
        [Display(Name="Godina izdanja")]
        public int GodinaIzdanja { get; set; }
        [Display(Name = "Broj strana")]
        public int BrojStrana { get; set; }
        public string ISBN { get; set; }
        public int JezikId { get; set; }
        public Jezik Jezik { get; set; }
        public ICollection<Autor> Autori { get; set; }
        public ICollection<PrimerakKnjige> PrimerciKnjige { get; set; }


    }
}
