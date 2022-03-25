using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class KreirajKnjiguVM
    {
        public Knjiga Knjiga { get; set; }
        public IEnumerable<SelectListItem> Autori { get; set; }
        public IEnumerable<SelectListItem> Jezici { get; set; }
        public IEnumerable<int> izabraniAutoriId { get; set; }

        public KreirajKnjiguVM(Knjiga knjiga, ICollection<Autor> autori, ICollection<Jezik> jezici)
        {
            this.Knjiga = knjiga;
            if (knjiga.Autori != null)
            {
                izabraniAutoriId = new List<int>(knjiga.Autori.Select(a => a.AutorId));
            }
            //this.Autori = autori;
            //this.Jezici = jezici;
            this.Autori = autori.Select(autor => new SelectListItem()
            {
                Value = autor.AutorId.ToString(),
                Text = autor.Ime + " " + autor.SrednjeIme + " " + autor.Prezime,
            });
            this.Jezici = jezici.Select(jezik => new SelectListItem()
            {
                Value = jezik.JezikId.ToString(),
                Text = jezik.Naziv,
            });
        }
        public KreirajKnjiguVM(Knjiga knjiga, IEnumerable<SelectListItem> autori, IEnumerable<SelectListItem> jezici)
        {
            this.Knjiga = knjiga;
            this.Autori = autori;
            this.Jezici = jezici;
        }
        public KreirajKnjiguVM()
        {

        }
    }
}
