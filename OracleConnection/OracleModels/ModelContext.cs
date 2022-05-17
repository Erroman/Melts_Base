using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OracleConnection.OracleModels
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Melt31> Melt31s { get; set; } = null!;
        public virtual DbSet<Melts31> Melts31s { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("User Id=MELTSBASE;Password=meltsbase31SS;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME= pdb1.mshome.net)));");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MELTSBASE")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<Melt31>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MELT31");

                entity.Property(e => e.DateClose)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_CLOSE");

                entity.Property(e => e.DateZap)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_ZAP");

                entity.Property(e => e.DemandOrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DEMAND_ORDER_ID");

                entity.Property(e => e.Dsd)
                    .HasColumnType("DATE")
                    .HasColumnName("DSD");

                entity.Property(e => e.Ins)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("INS");

                entity.Property(e => e.MfgOrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MFG_ORDER_ID");

                entity.Property(e => e.Ncp)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("NCP");

                entity.Property(e => e.Npart)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NPART");

                entity.Property(e => e.Npech)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("NPECH");

                entity.Property(e => e.Nplav)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NPLAV");

                entity.Property(e => e.OkonchPereplav)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OKONCH_PEREPLAV");

                entity.Property(e => e.PasportId)
                    .HasPrecision(10)
                    .HasColumnName("PASPORT_ID");

                entity.Property(e => e.Pereplav)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PEREPLAV");

                entity.Property(e => e.Poz)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("POZ");

                entity.Property(e => e.PozIl)
                    .IsUnicode(false)
                    .HasColumnName("POZ_IL");

                entity.Property(e => e.PozNaim)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POZ_NAIM");

                entity.Property(e => e.PozRazm)
                    .IsUnicode(false)
                    .HasColumnName("POZ_RAZM");

                entity.Property(e => e.RazmPasp)
                    .IsUnicode(false)
                    .HasColumnName("RAZM_PASP");

                entity.Property(e => e.RazmSdch)
                    .IsUnicode(false)
                    .HasColumnName("RAZM_SDCH");

                entity.Property(e => e.Splav)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("SPLAV");

                entity.Property(e => e.SumVesZapusk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SUM_VES_ZAPUSK");

                entity.Property(e => e.Tek)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TEK");

                entity.Property(e => e.VesSdch)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("VES_SDCH");

                entity.Property(e => e.Zapusk31)
                    .IsUnicode(false)
                    .HasColumnName("ZAPUSK_31");

                entity.Property(e => e.ZapuskNakl)
                    .IsUnicode(false)
                    .HasColumnName("ZAPUSK_NAKL");

                entity.Property(e => e.ZapuskPpf)
                    .HasMaxLength(44)
                    .IsUnicode(false)
                    .HasColumnName("ZAPUSK_PPF");
            });

            modelBuilder.Entity<Melts31>(entity =>
            {
                entity.HasKey(e => e.MeltId)
                    .HasName("MELTS31_PK");

                entity.ToTable("MELTS31");

                entity.Property(e => e.MeltId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MELT_ID");

                entity.Property(e => e.Alloyindex)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ALLOYINDEX");

                entity.Property(e => e.Alloyname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ALLOYNAME");

                entity.Property(e => e.Contract)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT");

                entity.Property(e => e.Electrodediameter)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ELECTRODEDIAMETER");

                entity.Property(e => e.IlUisShn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IL_UIS_SHN");

                entity.Property(e => e.Ingotdiameter)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("INGOTDIAMETER");

                entity.Property(e => e.Meltdate)
                    .HasColumnType("DATE")
                    .HasColumnName("MELTDATE");

                entity.Property(e => e.Meltername)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MELTERNAME");

                entity.Property(e => e.Melternumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MELTERNUMBER");

                entity.Property(e => e.Meltnumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MELTNUMBER");

                entity.Property(e => e.Mouldset)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOULDSET");

                entity.Property(e => e.Purpose)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PURPOSE");

                entity.Property(e => e.Supplement)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUPPLEMENT");

                entity.Property(e => e.Teknumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TEKNUMBER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
