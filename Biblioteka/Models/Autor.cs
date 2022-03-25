using Biblioteka.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class Autor
    {
        public int AutorId { get; set; }
        public string Ime { get; set; }
        [Display(Name ="Srednje ime")]
        public string SrednjeIme { get; set; }
        public string Prezime { get; set; }
        public Pol Pol { get; set; }
        public ICollection<Knjiga> Knjige { get; set; }

    }
}
