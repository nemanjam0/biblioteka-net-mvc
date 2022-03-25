using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteka.ViewModels
{
    public class PaginiranaListaVM<T> where T:new()//klasa tipa T mora da ima konstruktor koji ne prima parametre
    {
        public PaginacijaVM Paginator { get; set; }
        public T Podaci { get; set; } = new T();
        public PaginiranaListaVM(string URLTrenutneStrane,int brojTrenutneStranice,string NazivParametraZaStranu,T podaci)
        {
            this.Podaci = podaci;
            this.PodesiPaginator(URLTrenutneStrane, brojTrenutneStranice, NazivParametraZaStranu);
            
        }
        public void PodesiPaginator(string URLTrenutneStrane,int brojTrenutneStranice,string NazivParametraZaStranu)
        {
            var uri = new Uri(URLTrenutneStrane);
            var parametri = QueryHelpers.ParseQuery(uri.Query);//izvlacimo parametre iz URLa
            parametri.Remove(NazivParametraZaStranu);//brisemo parametar koji ukazuje na trenutnu stranu
            var url = uri.GetLeftPart(UriPartial.Path);//kreiramo novi url,ali bez ijednog parametra
            url = QueryHelpers.AddQueryString(url, parametri);//dodajemo sve parametre osim parametra koji ukazuje na trenutnu stranu
            string URLSledeceStrane = QueryHelpers.AddQueryString(url, NazivParametraZaStranu, (brojTrenutneStranice + 1).ToString());
            string URLPrethodneStrane = null;
            if (brojTrenutneStranice > 1)
            {
                URLPrethodneStrane = QueryHelpers.AddQueryString(url, NazivParametraZaStranu, (brojTrenutneStranice - 1).ToString());
            }
            this.Paginator = new PaginacijaVM(brojTrenutneStranice, URLPrethodneStrane, URLSledeceStrane);
        }
        public PaginiranaListaVM()
        {

        }
    }
}
