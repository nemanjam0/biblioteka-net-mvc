using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class PaginacijaVM
    {
        public int TrenutnaStrana { get; set; }
        public string URLPrethodneStrane { get; set; }
        public string URLSledeceStrane { get; set; }
        public readonly bool ImaSledecuStranu =false;
        public readonly bool ImaPrethodnuStranu=false;
        public PaginacijaVM(int trenutnaStrana, string uRLPrethodneStrane, string uRLSledeceStrane)
        {
            TrenutnaStrana = trenutnaStrana;
            URLPrethodneStrane = uRLPrethodneStrane;
            URLSledeceStrane = uRLSledeceStrane;
            if(URLPrethodneStrane is not null) ImaPrethodnuStranu = true;
            if (URLSledeceStrane is not null) ImaSledecuStranu = true;
        }
    }
}
