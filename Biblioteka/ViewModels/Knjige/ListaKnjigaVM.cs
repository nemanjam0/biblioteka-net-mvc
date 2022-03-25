using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class KnjigaUListiVM
    {
        public int KnjigaId { get; set; }
        [Display(Name ="Naziv")]
        public string NazivKnjige { get; set; }
        [Display(Name = "Izdanje")]
        public int IzdanjeKnjige { get; set; }
        [Display(Name = "Godina")]
        public int GodinaIzdanja { get; set; }
        [Display(Name = "Strana")]
        public int BrojStrana { get; set; }
        [Display(Name = "Jezik")]
        public string NazivJezika { get; set; }
        public string ISBN { get; set; }
        [Display(Name = "Br. primeraka")]
        public int BrojPrimeraka { get; set; }
        [Display(Name = "Br. iznajmljenih")]
        public int BrojIznajmljenihPrimeraka { get; set; }
        [Display(Name = "Autori")]
        public IEnumerable<Autor> AutoriKnjige { get; set; }

        public KnjigaUListiVM()
        {

        }
    }
}
