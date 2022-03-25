using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class PrimerakKnjigeVM
    {
        public PrimerakKnjige PrimerakKnjige { get; set; }
        public IEnumerable<SelectListItem> Knjige { get; set; }
        [Range(0,100)]
        [Display(Name ="Broj primeraka za dodavanje")]
        public int BrojPrimerakaZaDodavanje { get; set; }
        public PrimerakKnjigeVM(PrimerakKnjige primerakKnjige, ICollection<Knjiga> knjige)
        {
            this.PrimerakKnjige = primerakKnjige;
            this.Knjige = knjige.Select(knjiga => new SelectListItem()
            {
                Value = knjiga.KnjigaId.ToString(),
                Text = knjiga.Naziv + "(" + knjiga.Izdanje + ". izdanje)",
            });
        
            this.BrojPrimerakaZaDodavanje = 1;
        }
        public PrimerakKnjigeVM(PrimerakKnjige primerakKnjige, IEnumerable<SelectListItem> knjige)
        {
            this.PrimerakKnjige = primerakKnjige;
            this.Knjige = knjige;
            this.BrojPrimerakaZaDodavanje = 1;
        }
        public PrimerakKnjigeVM()
        {

        }
    }
}
