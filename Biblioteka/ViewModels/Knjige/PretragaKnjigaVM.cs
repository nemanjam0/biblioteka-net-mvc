using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class PretragaKnjigaVM
    {
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }

        [Display(Name = "Ime i prezime autora")]
        public string ImeAutora { get; set; }
        [Display(Name = "Izdanje")]
        public int? Izdanje { get; set; }

        [Display(Name = "ISBN")]
        public string ISBN { get; set; }
        [Display(Name = "Jezik")]
        public int? JezikId { get; set; }
        [Display(Name = "Strana")]
        public int Strana { get; set; } = 1;//Strana liste
        public IEnumerable<Jezik> Jezici { get; set; }
        public List<KnjigaUListiVM> Knjige { get; set; }
        public PretragaKnjigaVM()
        {

        }
    }
}
