using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class VratiKnjiguVM
    {
        [Display(Name="Broj knjige")]
        public int primerakKnjigeID { get; set; }
        public VratiKnjiguVM() { }
    }
}
