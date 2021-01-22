using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Models
{
    /// <summary>
    /// Parcijalna klasa RPPP06Context za inicijalizaiju pogleda tablica.
    /// Pogledi su stvoreni u bazi i koriste se u kontrolerima.
    /// </summary>
    public partial class RPPP06Context
    {
        public virtual DbSet<ViewPovjerenstvo> vw_Povjerenstvo { get; set; }
        public virtual DbSet<ViewPonuda> vw_Ponuda { get; set; }
        public virtual DbSet<ViewZapisnikInfo> vw_Zapisnici { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewPovjerenstvo>(entity =>
            {
                entity.HasNoKey();
                //entity.ToView("vw_Povjerenstvo");
            });
            modelBuilder.Entity<ViewZapisnikInfo>(entity =>
            {
                entity.HasNoKey();
                //entity.ToView("vw_Zapisnici");
            });
            modelBuilder.Entity<ViewPonuda>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
