using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class PrimerakKnjige
    {
        [Display(Name="Broj knjige")]
        public int PrimerakKnjigeId { get; set; }
        public Knjiga Knjiga { get; set; }
        public int KnjigaId { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name ="Vreme nabavke")]
        public DateTime VremeNabavke { get; set; } = DateTime.Now;
        public ICollection<Iznajmljivanje> Iznajmljivanja { get; set; }
    }
}
