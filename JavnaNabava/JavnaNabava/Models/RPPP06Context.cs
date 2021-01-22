using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class RPPP06Context : DbContext
    {
        public RPPP06Context()
        {
        }

        public RPPP06Context(DbContextOptions<RPPP06Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Cpv> Cpvs { get; set; }
        public virtual DbSet<Dokument> Dokuments { get; set; }
        public virtual DbSet<KontaktKonzorcij> KontaktKonzorcijs { get; set; }
        public virtual DbSet<KontaktPonuditelj> KontaktPonuditeljs { get; set; }
        public virtual DbSet<Konzorcij> Konzorcijs { get; set; }
        public virtual DbSet<KriterijZaVrednovanje> KriterijZaVrednovanjes { get; set; }
        public virtual DbSet<Naručitelj> Naručiteljs { get; set; }
        public virtual DbSet<Naručiteljovlaštenik> Naručiteljovlašteniks { get; set; }
        public virtual DbSet<Natječaj> Natječajs { get; set; }
        public virtual DbSet<Ovlaštenik> Ovlašteniks { get; set; }
        public virtual DbSet<Ovlaštenikpovjerenstvo> Ovlaštenikpovjerenstvos { get; set; }
        public virtual DbSet<PlanNabave> PlanNabaves { get; set; }
        public virtual DbSet<PonDokumenti> PonDokumentis { get; set; }
        public virtual DbSet<PonudaNatječaj> PonudaNatječajs { get; set; }
        public virtual DbSet<PonudaPonuditelj> PonudaPonuditeljs { get; set; }
        public virtual DbSet<PonudaStavke> PonudaStavkes { get; set; }
        public virtual DbSet<Ponuditelj> Ponuditeljs { get; set; }
        public virtual DbSet<Ponudum> Ponuda { get; set; }
        public virtual DbSet<Povjerenstvo> Povjerenstvos { get; set; }
        public virtual DbSet<Prilog> Prilogs { get; set; }
        public virtual DbSet<StatusNatječaja> StatusNatječajas { get; set; }
        public virtual DbSet<StavkaUTroškovniku> StavkaUTroškovnikus { get; set; }
        public virtual DbSet<StavkaZapisnik> StavkaZapisniks { get; set; }
        public virtual DbSet<Troškovnik> Troškovniks { get; set; }
        public virtual DbSet<TroškovnikStavka> TroškovnikStavkas { get; set; }
        public virtual DbSet<VrstaDjelatnosti> VrstaDjelatnostis { get; set; }
        public virtual DbSet<VrstaKompetencije> VrstaKompetencijes { get; set; }
        public virtual DbSet<VrstaKontaktum> VrstaKontakta { get; set; }
        public virtual DbSet<VrstaPostupka> VrstaPostupkas { get; set; }
        public virtual DbSet<VrstaStavke> VrstaStavkes { get; set; }
        public virtual DbSet<VrstaStručneSpreme> VrstaStručneSpremes { get; set; }
        public virtual DbSet<VwPonudum> VwPonuda { get; set; }
        public virtual DbSet<VwPovjerenstvo> VwPovjerenstvos { get; set; }
        public virtual DbSet<VwZapisnici> VwZapisnicis { get; set; }
        public virtual DbSet<Zapisnik> Zapisniks { get; set; }
        public virtual DbSet<OdredbaZapisnik> OdredbaZapisniks { get; set; }
        public virtual DbSet<Zaposlenik> Zaposleniks { get; set; }
        public virtual DbSet<ZaposlenikPrilog> ZaposlenikPrilogs { get; set; }
        public virtual DbSet<ČlanoviKonzorcija> ČlanoviKonzorcijas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Croatian_CI_AS");

            modelBuilder.Entity<Cpv>(entity =>
            {
                entity.HasKey(e => e.ŠifraCpv);

                entity.ToTable("CPV");

                entity.Property(e => e.ŠifraCpv)
                    .HasMaxLength(10)
                    .HasColumnName("šifraCPV");

                entity.Property(e => e.NazivCpv)
                    .HasMaxLength(200)
                    .HasColumnName("nazivCPV");
            });

            modelBuilder.Entity<Dokument>(entity =>
            {
                entity.ToTable("Dokument");

                entity.Property(e => e.DokumentId)
                    .ValueGeneratedNever()
                    .HasColumnName("dokumentID");

                entity.Property(e => e.Blob)
                    .IsRequired()
                    .HasColumnName("blob");

                entity.Property(e => e.DatumPredaje).HasColumnName("datumPredaje");

                entity.Property(e => e.Naslov)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("naslov")
                    .IsFixedLength(true);

                entity.Property(e => e.PonudaId).HasColumnName("ponudaID");

                entity.Property(e => e.Vrsta)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("vrsta")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<KontaktKonzorcij>(entity =>
            {
                entity.HasKey(e => new { e.IdKonzorcij, e.IdVrsteKontakta })
                    .HasName("PK__kontaktK__D12A6A8F3C6033C9");

                entity.ToTable("KontaktKonzorcij");

                entity.Property(e => e.IdKonzorcij).HasColumnName("idKonzorcij");

                entity.Property(e => e.IdVrsteKontakta).HasColumnName("idVrsteKontakta");

                entity.Property(e => e.KontaktK)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("kontaktK");

                entity.HasOne(d => d.IdKonzorcijNavigation)
                    .WithMany(p => p.KontaktKonzorcijs)
                    .HasForeignKey(d => d.IdKonzorcij)
                    .HasConstraintName("FK_kontaktKonzorcij_Konzorcij");

                entity.HasOne(d => d.IdVrsteKontaktaNavigation)
                    .WithMany(p => p.KontaktKonzorcijs)
                    .HasForeignKey(d => d.IdVrsteKontakta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_kontaktKonzorcij_VrstaKontakta");
            });

            modelBuilder.Entity<KontaktPonuditelj>(entity =>
            {
                entity.HasKey(e => new { e.OibPonuditelj, e.IdVrsteKontakta })
                    .HasName("PK__kontaktP__D9BD86D48CF06C68");

                entity.ToTable("KontaktPonuditelj");

                entity.Property(e => e.OibPonuditelj)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("oibPonuditelj")
                    .IsFixedLength(true);

                entity.Property(e => e.IdVrsteKontakta).HasColumnName("idVrsteKontakta");

                entity.Property(e => e.KontaktP)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("kontaktP");

                entity.HasOne(d => d.IdVrsteKontaktaNavigation)
                    .WithMany(p => p.KontaktPonuditeljs)
                    .HasForeignKey(d => d.IdVrsteKontakta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_kontaktPonuditelj_VrstaKontakta");

                entity.HasOne(d => d.OibPonuditeljNavigation)
                    .WithMany(p => p.KontaktPonuditeljs)
                    .HasForeignKey(d => d.OibPonuditelj)
                    .HasConstraintName("FK_kontaktPonuditelj_Ponuditelj");
            });

            modelBuilder.Entity<Konzorcij>(entity =>
            {
                entity.HasKey(e => e.IdKonzorcij);

                entity.ToTable("Konzorcij");

                entity.Property(e => e.IdKonzorcij)
                    .ValueGeneratedNever()
                    .HasColumnName("idKonzorcij");

                entity.Property(e => e.NazivKonzorcij)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivKonzorcij");
            });

            modelBuilder.Entity<KriterijZaVrednovanje>(entity =>
            {
                entity.HasKey(e => e.IdKriterija)
                    .HasName("PK_KRITERIJ ZA VREDNOVANJE");

                entity.ToTable("KriterijZaVrednovanje");

                entity.Property(e => e.IdKriterija)
                    .ValueGeneratedNever()
                    .HasColumnName("idKriterija");

                entity.Property(e => e.NazivKriterija)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("nazivKriterija");
            });

            modelBuilder.Entity<Naručitelj>(entity =>
            {
                entity.HasKey(e => e.OibNaručitelja)
                    .HasName("PK_oibNaručitelja");

                entity.ToTable("Naručitelj");

                entity.Property(e => e.OibNaručitelja)
                    .HasMaxLength(11)
                    .HasColumnName("oibNaručitelja");

                entity.Property(e => e.AdresaNaručitelja)
                    .IsRequired()
                    .HasMaxLength(350)
                    .HasColumnName("adresaNaručitelja");

                entity.Property(e => e.NazivNaručitelja)
                    .IsRequired()
                    .HasMaxLength(350)
                    .HasColumnName("nazivNaručitelja");

                entity.Property(e => e.PoštanskiBrojNaručitelja).HasColumnName("poštanskiBrojNaručitelja");
            });

            modelBuilder.Entity<Naručiteljovlaštenik>(entity =>
            {
                entity.HasKey(e => new { e.OibOvlaštenik, e.OibNaručitelja });

                entity.ToTable("naručiteljovlaštenik");

                entity.Property(e => e.OibOvlaštenik).HasColumnName("oibOvlaštenik");

                entity.Property(e => e.OibNaručitelja)
                    .HasMaxLength(11)
                    .HasColumnName("oibNaručitelja");

                entity.HasOne(d => d.OibNaručiteljaNavigation)
                    .WithMany(p => p.Naručiteljovlašteniks)
                    .HasForeignKey(d => d.OibNaručitelja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_naručiteljovlaštenik_Naručitelj");

                entity.HasOne(d => d.OibOvlaštenikNavigation)
                    .WithMany(p => p.Naručiteljovlašteniks)
                    .HasForeignKey(d => d.OibOvlaštenik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_naručiteljovlaštenik_Ovlaštenik");
            });

            modelBuilder.Entity<Natječaj>(entity =>
            {
                entity.HasKey(e => e.EvidBrojNatječaj)
                    .HasName("PK_NATJEČAJ");

                entity.ToTable("Natječaj");

                entity.Property(e => e.EvidBrojNatječaj)
                    .ValueGeneratedNever()
                    .HasColumnName("evidBrojNatječaj");

                entity.Property(e => e.EvidBrojPlan).HasColumnName("evidBrojPlan");

                entity.Property(e => e.IdKriterija).HasColumnName("idKriterija");

                entity.Property(e => e.IdStatusaNatječaja).HasColumnName("idStatusaNatječaja");

                entity.Property(e => e.IdVrstePostupka).HasColumnName("idVrstePostupka");

                entity.Property(e => e.LimitNatječaja)
                    .HasColumnType("money")
                    .HasColumnName("limitNatječaja");

                entity.Property(e => e.NazivNatječaja)
                    .HasMaxLength(350)
                    .HasColumnName("nazivNatječaja");

                entity.Property(e => e.RokZaDostavu).HasColumnName("rokZaDostavu");

                entity.Property(e => e.TrajanjeUgovora).HasColumnName("trajanjeUgovora");

                entity.HasOne(d => d.EvidBrojPlanNavigation)
                    .WithMany(p => p.Natječajs)
                    .HasForeignKey(d => d.EvidBrojPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natječaj_PlanNabave");

                entity.HasOne(d => d.IdKriterijaNavigation)
                    .WithMany(p => p.Natječajs)
                    .HasForeignKey(d => d.IdKriterija)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natječaj_KriterijZaVrednovanje");

                entity.HasOne(d => d.IdStatusaNatječajaNavigation)
                    .WithMany(p => p.Natječajs)
                    .HasForeignKey(d => d.IdStatusaNatječaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natječaj_StatusNatječaja");

                entity.HasOne(d => d.IdVrstePostupkaNavigation)
                    .WithMany(p => p.Natječajs)
                    .HasForeignKey(d => d.IdVrstePostupka)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natječaj_VrstaPostupka");
            });

            modelBuilder.Entity<Ovlaštenik>(entity =>
            {
                entity.HasKey(e => e.OibOvlaštenik)
                    .HasName("PK_Ovlaštenik2");

                entity.ToTable("Ovlaštenik");

                entity.Property(e => e.OibOvlaštenik)
                    .ValueGeneratedNever()
                    .HasColumnName("oibOvlaštenik");

                entity.Property(e => e.IdPovjerenstva).HasColumnName("idPovjerenstva");

                entity.Property(e => e.ImeOvlaštenik)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("imeOvlaštenik");

                entity.Property(e => e.OibNaručitelja)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("oibNaručitelja");

                entity.Property(e => e.PrezimeOvlaštenik)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("prezimeOvlaštenik");
            });

            modelBuilder.Entity<Ovlaštenikpovjerenstvo>(entity =>
            {
                entity.HasKey(e => new { e.IdPovjerenstva, e.OibOvlaštenik });

                entity.ToTable("ovlaštenikpovjerenstvo");

                entity.Property(e => e.IdPovjerenstva).HasColumnName("idPovjerenstva");

                entity.Property(e => e.OibOvlaštenik).HasColumnName("oibOvlaštenik");

                entity.HasOne(d => d.IdPovjerenstvaNavigation)
                    .WithMany(p => p.Ovlaštenikpovjerenstvos)
                    .HasForeignKey(d => d.IdPovjerenstva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ovlaštenikpovjerenstvo_Povjerenstvo");

                entity.HasOne(d => d.OibOvlaštenikNavigation)
                    .WithMany(p => p.Ovlaštenikpovjerenstvos)
                    .HasForeignKey(d => d.OibOvlaštenik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ovlaštenikpovjerenstvo_Ovlaštenik");
            });

            modelBuilder.Entity<PlanNabave>(entity =>
            {
                entity.HasKey(e => e.EvidBrojPlan)
                    .HasName("PK_PLAN_NABAVE");

                entity.ToTable("PlanNabave");

                entity.Property(e => e.EvidBrojPlan)
                    .ValueGeneratedNever()
                    .HasColumnName("evidBrojPlan");

                entity.Property(e => e.IdDjelatnosti).HasColumnName("idDjelatnosti");

                entity.Property(e => e.OibNaručitelja)
                    .HasMaxLength(11)
                    .HasColumnName("oibNaručitelja");

                entity.Property(e => e.TrajanjeNabave)
                    .HasColumnType("datetime")
                    .HasColumnName("trajanjeNabave");

                entity.Property(e => e.VrijednostNabave)
                    .HasColumnType("money")
                    .HasColumnName("vrijednostNabave");

                entity.Property(e => e.ŠifraCpv)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("šifraCPV");

                entity.HasOne(d => d.IdDjelatnostiNavigation)
                    .WithMany(p => p.PlanNabaves)
                    .HasForeignKey(d => d.IdDjelatnosti)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanNabave_VrstaDjelatnosti");

                entity.HasOne(d => d.OibNaručiteljaNavigation)
                    .WithMany(p => p.PlanNabaves)
                    .HasForeignKey(d => d.OibNaručitelja)
                    .HasConstraintName("FK_PlanNabave_Naručitelj2");

                entity.HasOne(d => d.ŠifraCpvNavigation)
                    .WithMany(p => p.PlanNabaves)
                    .HasForeignKey(d => d.ŠifraCpv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanNabave_CPV");
            });

            modelBuilder.Entity<PonDokumenti>(entity =>
            {
                entity.HasKey(e => new { e.PonudaId, e.DokumentId });

                entity.ToTable("ponDokumenti");

                entity.Property(e => e.PonudaId).HasColumnName("ponudaID");

                entity.Property(e => e.DokumentId).HasColumnName("dokumentID");

                entity.HasOne(d => d.Dokument)
                    .WithMany(p => p.PonDokumentis)
                    .HasForeignKey(d => d.DokumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponDokumenti_Dokument");

                entity.HasOne(d => d.Ponuda)
                    .WithMany(p => p.PonDokumentis)
                    .HasForeignKey(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponDokumenti_PONUDA");
            });

            modelBuilder.Entity<PonudaNatječaj>(entity =>
            {
                entity.HasKey(e => new { e.EvidBrojNatječaj, e.PonudaId });

                entity.ToTable("ponudaNatječaj");

                entity.Property(e => e.EvidBrojNatječaj).HasColumnName("evidBrojNatječaj");

                entity.Property(e => e.PonudaId).HasColumnName("ponudaID");

                entity.HasOne(d => d.EvidBrojNatječajNavigation)
                    .WithMany(p => p.PonudaNatječajs)
                    .HasForeignKey(d => d.EvidBrojNatječaj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponudaNatječaj_Natječaj");

                entity.HasOne(d => d.Ponuda)
                    .WithMany(p => p.PonudaNatječajs)
                    .HasForeignKey(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponudaNatječaj_PONUDA");
            });

            modelBuilder.Entity<PonudaPonuditelj>(entity =>
            {
                entity.HasKey(e => e.PonudaId);

                entity.ToTable("ponudaPonuditelj");

                entity.Property(e => e.PonudaId)
                    .ValueGeneratedNever()
                    .HasColumnName("ponudaID");

                entity.Property(e => e.OibPonuditelj)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("oibPonuditelj")
                    .IsFixedLength(true);

                entity.HasOne(d => d.OibPonuditeljNavigation)
                    .WithMany(p => p.PonudaPonuditeljs)
                    .HasForeignKey(d => d.OibPonuditelj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponudaPonuditelj_Ponuditelj");

                entity.HasOne(d => d.Ponuda)
                    .WithOne(p => p.PonudaPonuditelj)
                    .HasForeignKey<PonudaPonuditelj>(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponudaPonuditelj_PONUDA");
            });

            modelBuilder.Entity<PonudaStavke>(entity =>
            {
                entity.HasKey(e => e.PonudaId);

                entity.ToTable("ponuda_Stavke");

                entity.Property(e => e.PonudaId)
                    .ValueGeneratedNever()
                    .HasColumnName("ponudaID");

                entity.Property(e => e.CijenaStavke)
                    .HasColumnType("money")
                    .HasColumnName("cijenaStavke");

                entity.Property(e => e.KoličinaStavke).HasColumnName("količinaStavke");

                entity.HasOne(d => d.IdStavkeNavigation)
                    .WithMany(p => p.PonudaStavkes)
                    .HasForeignKey(d => d.IdStavke)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponuda_Stavke_Stavka_u_troškovniku");

                entity.HasOne(d => d.Ponuda)
                    .WithOne(p => p.PonudaStavke)
                    .HasForeignKey<PonudaStavke>(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ponuda_Stavke_PONUDA");
            });

            modelBuilder.Entity<Ponuditelj>(entity =>
            {
                entity.HasKey(e => e.OibPonuditelj);

                entity.ToTable("Ponuditelj");

                entity.Property(e => e.OibPonuditelj)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("oibPonuditelj")
                    .IsFixedLength(true);

                entity.Property(e => e.AdresaPonuditelj)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adresaPonuditelj");

                entity.Property(e => e.NazivPonuditelj)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivPonuditelj");

                entity.Property(e => e.SjedištePonuditelj)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sjedištePonuditelj");
            });

            modelBuilder.Entity<Ponudum>(entity =>
            {
                entity.HasKey(e => e.PonudaId)
                    .HasName("PK_PONUDA");

                entity.Property(e => e.PonudaId)
                    .ValueGeneratedNever()
                    .HasColumnName("ponudaID");

                entity.Property(e => e.EvidBrojNatječaj).HasColumnName("evidBrojNatječaj");

                entity.Property(e => e.Naslov).HasMaxLength(50);

                entity.Property(e => e.OibPonuditelj)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("oibPonuditelj")
                    .IsFixedLength(true);

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("text");
            });

            modelBuilder.Entity<Povjerenstvo>(entity =>
            {
                entity.HasKey(e => e.IdPovjerenstva)
                    .HasName("PK_Povjerenstvo2");

                entity.ToTable("Povjerenstvo");

                entity.Property(e => e.IdPovjerenstva)
                    .ValueGeneratedNever()
                    .HasColumnName("idPovjerenstva");

                entity.Property(e => e.EvidBrojNatječaj).HasColumnName("evidBrojNatječaj");

                entity.Property(e => e.NazivPovjerenstva)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nazivPovjerenstva");

                entity.HasOne(d => d.EvidBrojNatječajNavigation)
                    .WithMany(p => p.Povjerenstvos)
                    .HasForeignKey(d => d.EvidBrojNatječaj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Povjerenstvo_Natječaj");
            });

            modelBuilder.Entity<Prilog>(entity =>
            {
                entity.HasKey(e => e.IdPrilog);

                entity.ToTable("Prilog");

                entity.Property(e => e.IdPrilog)
                    .ValueGeneratedNever()
                    .HasColumnName("idPrilog");

                entity.Property(e => e.NazivPrilog)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("nazivPrilog");
            });

            modelBuilder.Entity<StatusNatječaja>(entity =>
            {
                entity.HasKey(e => e.IdStatusaNatječaja)
                    .HasName("PK_StatusNaručitelja");

                entity.ToTable("StatusNatječaja");

                entity.Property(e => e.IdStatusaNatječaja)
                    .ValueGeneratedNever()
                    .HasColumnName("idStatusaNatječaja");

                entity.Property(e => e.NazivStatusaNatječaja)
                    .HasMaxLength(350)
                    .HasColumnName("nazivStatusaNatječaja");
            });

            modelBuilder.Entity<StavkaUTroškovniku>(entity =>
            {
                entity.HasKey(e => e.IdStavke)
                    .HasName("PK__Stavka_u__B6DF3ABB448608EC");

                entity.ToTable("Stavka_u_troškovniku");

                entity.Property(e => e.IdStavke).ValueGeneratedNever();

                entity.Property(e => e.DodatneInformacije).HasMaxLength(200);

                entity.Property(e => e.NazivStavke).HasMaxLength(200);

                entity.HasOne(d => d.IdVrsteNavigation)
                    .WithMany(p => p.StavkaUTroškovnikus)
                    .HasForeignKey(d => d.IdVrste)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stavka_u_troškovniku_VrstaStavke");
            });

            modelBuilder.Entity<StavkaZapisnik>(entity =>
            {
                entity.HasKey(e => e.idStavke);

                entity.ToTable("StavkaZapisnik");

                entity.Property(e => e.idStavke)
                    .ValueGeneratedNever()
                    .HasColumnName("idStavke");

                entity.Property(e => e.zapisnikID).HasColumnName("zapisnikID");

                entity.Property(e => e.idOdredba).HasColumnName("idOdredba");

                entity.Property(e => e.ispravnostStavka)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ispravnostStavka");

                entity.Property(e => e.cijenaKršenja)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cijenaKršenja");

                entity.HasOne(d => d.Zapisnik)
                    .WithMany(p => p.StavkaZapisniks)
                    .HasForeignKey(d => d.zapisnikID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StavkaZapisnik_Zapisnik");

                entity.HasOne(d => d.OdredbaZapisnik)
                    .WithMany(p => p.StavkaZapisniks)
                    .HasForeignKey(d => d.idOdredba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StavkaZapisnik_OdredbaZapisnik");
            });

            modelBuilder.Entity<OdredbaZapisnik>(entity =>
            {
                entity.HasKey(e => e.idOdredba);

                entity.ToTable("OdredbaZapisnik");

                entity.Property(e => e.idOdredba)
                    .ValueGeneratedNever()
                    .HasColumnName("idOdredba");

                entity.Property(e => e.nazivOdredba)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nazivOdredba");

                entity.Property(e => e.uvjetOdredba)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("uvjetOdredba");

                entity.Property(e => e.tekstOdredba)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("tekstOdredba");

            });

            modelBuilder.Entity<Troškovnik>(entity =>
            {
                entity.ToTable("Troškovnik");

                entity.HasIndex(e => e.EvidBrojNatječaj, "UNIQUE_evidBrojNatječaj")
                    .IsUnique();

                entity.Property(e => e.TroškovnikId)
                    .ValueGeneratedNever()
                    .HasColumnName("troškovnikID");

                entity.Property(e => e.EvidBrojNatječaj).HasColumnName("evidBrojNatječaj");

                entity.HasOne(d => d.EvidBrojNatječajNavigation)
                    .WithOne(p => p.Troškovnik)
                    .HasForeignKey<Troškovnik>(d => d.EvidBrojNatječaj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Troškovnik_Natječaj");
            });

            modelBuilder.Entity<TroškovnikStavka>(entity =>
            {
                entity.HasKey(e => new { e.TroškovnikId, e.IdStavke })
                    .HasName("PK__Troškovn__A333593243182950");

                entity.ToTable("TroškovnikStavka");

                entity.Property(e => e.TroškovnikId).HasColumnName("troškovnikID");

                entity.HasOne(d => d.IdStavkeNavigation)
                    .WithMany(p => p.TroškovnikStavkas)
                    .HasForeignKey(d => d.IdStavke)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TroškovnikStavka_Stavka_u_troškovniku");

                entity.HasOne(d => d.Troškovnik)
                    .WithMany(p => p.TroškovnikStavkas)
                    .HasForeignKey(d => d.TroškovnikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Troškovni__trošk__04E4BC85");
            });

            modelBuilder.Entity<VrstaDjelatnosti>(entity =>
            {
                entity.HasKey(e => e.IdDjelatnosti)
                    .HasName("PK_VRSTA DJELATNOSTI");

                entity.ToTable("VrstaDjelatnosti");

                entity.Property(e => e.IdDjelatnosti)
                    .ValueGeneratedNever()
                    .HasColumnName("idDjelatnosti");

                entity.Property(e => e.NazivDjelatnosti)
                    .HasMaxLength(350)
                    .HasColumnName("nazivDjelatnosti");
            });

            modelBuilder.Entity<VrstaKompetencije>(entity =>
            {
                entity.HasKey(e => e.IdKompetencije);

                entity.ToTable("VrstaKompetencije");

                entity.Property(e => e.IdKompetencije)
                    .ValueGeneratedNever()
                    .HasColumnName("idKompetencije");

                entity.Property(e => e.NazivKompetencije)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("nazivKompetencije");
                entity.Property(e => e.SjedišteObrazovneUstanove)
                    .HasMaxLength(500)
                    .HasColumnName("sjedišteObrazovneUstanove");
                entity.Property(e => e.PočetakObrazovanja).HasColumnName("početakObrazovanja");
                entity.Property(e => e.KrajObrazovanja).HasColumnName("krajObrazovanja");
            });

            modelBuilder.Entity<VrstaKontaktum>(entity =>
            {
                entity.HasKey(e => e.IdVrstaKontakta);

                entity.Property(e => e.IdVrstaKontakta)
                    .ValueGeneratedNever()
                    .HasColumnName("idVrstaKontakta");

                entity.Property(e => e.NazivVrstaKontakta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivVrstaKontakta");
            });

            modelBuilder.Entity<VrstaPostupka>(entity =>
            {
                entity.HasKey(e => e.IdVrstePostupka)
                    .HasName("PK_VRSTA POSTUPKA");

                entity.ToTable("VrstaPostupka");

                entity.Property(e => e.IdVrstePostupka)
                    .ValueGeneratedNever()
                    .HasColumnName("idVrstePostupka");

                entity.Property(e => e.NazivVrste)
                    .HasMaxLength(350)
                    .HasColumnName("nazivVrste");
            });

            modelBuilder.Entity<VrstaStavke>(entity =>
            {
                entity.HasKey(e => e.IdVrste)
                    .HasName("PK__VrstaSta__5FB7EC17C9CC711B");

                entity.ToTable("VrstaStavke");

                entity.Property(e => e.IdVrste).ValueGeneratedNever();

                entity.Property(e => e.NazivVrste).HasMaxLength(200);
            });

            modelBuilder.Entity<VrstaStručneSpreme>(entity =>
            {
                entity.HasKey(e => e.IdStručneSpreme);

                entity.ToTable("VrstaStručneSpreme");

                entity.Property(e => e.IdStručneSpreme)
                    .ValueGeneratedNever()
                    .HasColumnName("idStručneSpreme");

                entity.Property(e => e.NazivStručneSpreme)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("nazivStručneSpreme");
            });

            modelBuilder.Entity<VwPonudum>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Ponuda");

                entity.Property(e => e.PonudaId).HasColumnName("ponudaID");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("text");
            });

            modelBuilder.Entity<VwPovjerenstvo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Povjerenstvo");

                entity.Property(e => e.IdPovjerenstva).HasColumnName("idPovjerenstva");

                entity.Property(e => e.NazivPovjerenstva)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nazivPovjerenstva");
            });

            modelBuilder.Entity<VwZapisnici>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Zapisnici");

                entity.Property(e => e.IdPovjerenstva).HasColumnName("idPovjerenstva");

                entity.Property(e => e.IdPrethZapisnika).HasColumnName("idPrethZapisnika");

                entity.Property(e => e.NazivPonude).HasMaxLength(50);

                entity.Property(e => e.NazivPovjerenstva)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NazivZapisnik)
                    .HasMaxLength(50)
                    .HasColumnName("nazivZapisnik");

                entity.Property(e => e.PonudaId).HasColumnName("ponudaID");

                entity.Property(e => e.ZapisnikId).HasColumnName("zapisnikID");
            });

            modelBuilder.Entity<Zapisnik>(entity =>
            {
                entity.ToTable("Zapisnik");

                entity.Property(e => e.ZapisnikId)
                    .ValueGeneratedNever()
                    .HasColumnName("zapisnikID");

                entity.Property(e => e.IdPovjerenstva).HasColumnName("idPovjerenstva");

                entity.Property(e => e.IdPrethZapisnika).HasColumnName("idPrethZapisnika");

                entity.Property(e => e.NazivZapisnik)
                    .HasMaxLength(50)
                    .HasColumnName("nazivZapisnik");

                entity.Property(e => e.PonudaId).HasColumnName("ponudaID");

                entity.Property(e => e.IspravnostZapisnik).HasColumnName("ispravnostZapisnik");

                entity.HasOne(d => d.IdPovjerenstvaNavigation)
                    .WithMany(p => p.Zapisniks)
                    .HasForeignKey(d => d.IdPovjerenstva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zapisnik_Povjerenstvo");

                entity.HasOne(d => d.Ponuda)
                    .WithMany(p => p.Zapisniks)
                    .HasForeignKey(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zapisnik_Ponuda");
            });

            modelBuilder.Entity<Zaposlenik>(entity =>
            {
                entity.HasKey(e => e.OibZaposlenik);

                entity.ToTable("Zaposlenik");

                entity.Property(e => e.OibZaposlenik)
                    .HasMaxLength(11)
                    .HasColumnName("oibZaposlenik");

                entity.Property(e => e.DatumRođenja).HasColumnName("datumRođenja");

                entity.Property(e => e.IdKompetencije).HasColumnName("idKompetencije");

                entity.Property(e => e.IdStručneSpreme).HasColumnName("idStručneSpreme");

                entity.Property(e => e.ImeZaposlenik)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("imeZaposlenik");

                entity.Property(e => e.MjestoPrebivališta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("mjestoPrebivališta");

                entity.Property(e => e.OibPonuditelj)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("oibPonuditelj")
                    .IsFixedLength(true);

                entity.Property(e => e.PrezimeZaposlenik)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("prezimeZaposlenik");

                entity.HasOne(d => d.IdKompetencijeNavigation)
                    .WithMany(p => p.Zaposleniks)
                    .HasForeignKey(d => d.IdKompetencije)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zaposlenik_VrstaKompetencije");

                entity.HasOne(d => d.IdStručneSpremeNavigation)
                    .WithMany(p => p.Zaposleniks)
                    .HasForeignKey(d => d.IdStručneSpreme)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zaposlenik_VrstaStručneSpreme");

                entity.HasOne(d => d.OibPonuditeljNavigation)
                    .WithMany(p => p.Zaposleniks)
                    .HasForeignKey(d => d.OibPonuditelj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zaposlenik_Ponuditelj");
            });

            modelBuilder.Entity<ZaposlenikPrilog>(entity =>
            {
                entity.HasKey(e => new { e.OibZaposlenik, e.IdPrilog });

                entity.ToTable("zaposlenikPrilog");

                entity.Property(e => e.OibZaposlenik)
                    .HasMaxLength(11)
                    .HasColumnName("oibZaposlenik");

                entity.Property(e => e.IdPrilog).HasColumnName("idPrilog");

                entity.Property(e => e.LinkNaPrilog)
                    .HasMaxLength(500)
                    .HasColumnName("linkNaPrilog");

                entity.Property(e => e.VrijediDo).HasColumnName("vrijediDo");

                entity.Property(e => e.VrijediOd).HasColumnName("vrijediOd");

                entity.HasOne(d => d.IdPrilogNavigation)
                    .WithMany(p => p.ZaposlenikPrilogs)
                    .HasForeignKey(d => d.IdPrilog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_zaposlenikPrilog_Prilog");

                entity.HasOne(d => d.OibZaposlenikNavigation)
                    .WithMany(p => p.ZaposlenikPrilogs)
                    .HasForeignKey(d => d.OibZaposlenik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_zaposlenikPrilog_Zaposlenik");
            });

            modelBuilder.Entity<ČlanoviKonzorcija>(entity =>
            {
                entity.HasKey(e => new { e.OibPonuditelj, e.IdKonzorcij })
                    .HasName("PK__članoviK__6D641434395F4FBD");

                entity.ToTable("ČlanoviKonzorcija");

                entity.Property(e => e.OibPonuditelj)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("oibPonuditelj")
                    .IsFixedLength(true);

                entity.Property(e => e.IdKonzorcij).HasColumnName("idKonzorcij");

                entity.Property(e => e.JeLiVoditelj).HasColumnName("jeLiVoditelj");

                entity.HasOne(d => d.IdKonzorcijNavigation)
                    .WithMany(p => p.ČlanoviKonzorcijas)
                    .HasForeignKey(d => d.IdKonzorcij)
                    .HasConstraintName("FK_članoviKonzorcija_Konzorcij");

                entity.HasOne(d => d.OibPonuditeljNavigation)
                    .WithMany(p => p.ČlanoviKonzorcijas)
                    .HasForeignKey(d => d.OibPonuditelj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_članoviKonzorcija_Ponuditelj");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
