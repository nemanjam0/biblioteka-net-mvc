using Biblioteka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteka.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik,TipKorisnika,int>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Autor> Autori { get; set; }
        public DbSet<Jezik> Jezici { get; set; }
        public DbSet<Knjiga> Knjige { get; set; }
        public DbSet<PrimerakKnjige> PrimerciKnjige { get; set; }
        public DbSet<Iznajmljivanje> Iznajmljivanja { get; set; }
        public DbSet<TipKorisnika> TipoviKorisnika { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Korisnik>().HasMany(korisnik => korisnik.Iznajmljivanja).WithOne(iznajmljivanje => iznajmljivanje.Korisnik);
            modelBuilder.Entity<Korisnik>().HasMany(korisnik => korisnik.Izdavanja).WithOne(iznajmljivanje => iznajmljivanje.Bibliotekar);
            modelBuilder.Entity<Iznajmljivanje>().HasOne(iznajmljivanje => iznajmljivanje.Korisnik).WithMany(korisnik => korisnik.Iznajmljivanja).HasForeignKey(Iznajmljivanje=>Iznajmljivanje.KorisnikId);
            modelBuilder.Entity<TipKorisnika>().HasData(new TipKorisnika
            {
                Id=1,
                Name = "Korisnik",
                NormalizedName = "KORISNIK"
            }, new TipKorisnika
            {
                Id=2,
                Name = "Bibliotekar",
                NormalizedName = "BIBLIOTEKAR"
            },
            new TipKorisnika
            {
                Id = 3,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

           
            
        }
    }
}
