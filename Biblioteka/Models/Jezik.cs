using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class Jezik
    {
        public int JezikId { get; set; }
        public string Naziv { get; set; }
        public string Skracenica { get; set; }
        public ICollection<Knjiga> Knjige { get; set; }
    }
}
