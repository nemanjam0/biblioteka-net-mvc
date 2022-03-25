using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class ProduziClanskuKartuVM
    {
        [Display(Name="Broj članske karte")]
        public int korisnikId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime? RokVazenja { get; set; }
        
        public ProduziClanskuKartuVM() { }
    }
}
