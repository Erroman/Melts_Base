using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Melts_Base.OracleModels
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

        public virtual DbSet<Melts31> Melts31s { get; set; }

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
                    .IsRequired()
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
