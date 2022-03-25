using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class ListaPrimerakaKnjigeVM
    {
        [Display(Name ="Broj knjige")]
        public int PrimerakKnjigaId { get; set; }
        [Display(Name ="Naziv")]
        public string NazivKnjige { get; set; }
        [Display(Name = "Izdanje")]
        public int IzdanjeKnjige { get; set; }
        [Display(Name = "Vreme nabavke")]
        public DateTime VremeNabavke { get; set; }
        [Display(Name = "Ime korisnika")]
        public string ImeKorisnika { get; set; }
        [Display(Name = "Ime bibliotekara")]
        public string ImeBibliotekara { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MMM.yyyy. HH:mm}")]
        [Display(Name = "Vreme uzimanja")]
        public DateTime VremeUzimanja { get; set; }
        [Display(Name = "Rok vraćanja")]
        [DisplayFormat(DataFormatString = "{0:dd.MMM.yyyy.}")]
        public DateTime RokVracanja { get; set; }
        [Display(Name = "Vreme vraćanja")]
        [DisplayFormat(DataFormatString = "{0:dd.MMM.yyyy.}")]
        public DateTime? DatumVracanja { get; set; }

        public ListaPrimerakaKnjigeVM()
        {

        }
    }
}
