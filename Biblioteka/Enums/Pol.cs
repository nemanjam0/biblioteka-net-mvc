using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Enums
{
    public enum Pol
    {
        [Display(Name ="Muški")]
        Muski,
        [Display(Name = "Ženski")]
        Zenski
    }
}
