using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Melts_Base.PostgresFiles
{
    public partial class epasportContext : DbContext
    {
        public epasportContext()
        {
        }

        public epasportContext(DbContextOptions<epasportContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Epasport> Epasports { get; set; }
        public virtual DbSet<MeltPostgres> Melts { get; set; }
        public virtual DbSet<SprGlobalId> SprGlobalIds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=10.10.48.24;Port=5432;Database=epasport;Username=romanovskii;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Epasport>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("epasport");

                entity.Property(e => e.ArcCurrent)
                    .HasColumnName("arc_current")
                    .HasComment("Ток дуги");

                entity.Property(e => e.ArcVoltage)
                    .HasColumnName("arc_voltage")
                    .HasComment("Напряжение дуги");

                entity.Property(e => e.ArcVoltageMax)
                    .HasColumnName("arc_voltage_max")
                    .HasComment("Диапазон напряжения максимум");

                entity.Property(e => e.ArcVoltageMin)
                    .HasColumnName("arc_voltage_min")
                    .HasComment("Диапазон напряжения минимум");

                entity.Property(e => e.AsutpPaspId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("asutp_pasp_id")
                    .HasComment("Идентификатор записи");

                entity.Property(e => e.CoolingEnv)
                    .HasColumnName("cooling_env")
                    .HasComment("Средя в которой происходит охлаждение: 1-гелий, 2-аргон");

                entity.Property(e => e.CoolingStartTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("cooling_start_time")
                    .HasComment("Время начала охлаждения");

                entity.Property(e => e.CoolingStopTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("cooling_stop_time")
                    .HasComment("Время окончания охлаждения");

                entity.Property(e => e.CoolingTotalTime)
                    .HasColumnName("cooling_total_time")
                    .HasComment("Фактическое время охлаждения");

                entity.Property(e => e.GlobalId)
                    .HasColumnName("global_id")
                    .HasComment("ID оборудования GLOBAL");

                entity.Property(e => e.LtLeak)
                    .HasColumnName("lt_leak")
                    .HasComment("Фактическое натекание");

                entity.Property(e => e.LtStartPres)
                    .HasColumnName("lt_start_pres")
                    .HasComment("Остаточное давление в начале проверки натекания");

                entity.Property(e => e.LtStartTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("lt_start_time")
                    .HasComment("Время начала проверки натекания");

                entity.Property(e => e.LtStopPres)
                    .HasColumnName("lt_stop_pres")
                    .HasComment("Остаточное давление в конце проверки натекания");

                entity.Property(e => e.LtStopTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("lt_stop_time")
                    .HasComment("Время окончания проверки натекания");

                entity.Property(e => e.MeltNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("melt_number")
                    .HasComment("Номер плавки");

                entity.Property(e => e.MeltPressure)
                    .HasColumnName("melt_pressure")
                    .HasComment("Остаточное давление факт, мм рт.ст.");

                entity.Property(e => e.MeltStartTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("melt_start_time")
                    .HasComment("Время начала плавления");

                entity.Property(e => e.MeltTotalTime)
                    .HasColumnName("melt_total_time")
                    .HasComment("Фактическое время плавки");

                entity.Property(e => e.OperFinished)
                    .IsRequired()
                    .HasColumnName("oper_finished")
                    .HasDefaultValueSql("true")
                    .HasComment("Признак окончания операции");

                entity.Property(e => e.OperType)
                    .HasColumnName("oper_type")
                    .HasComment("Тип операции: 1-приварка, 3-плавка, 5-оплавление");

                entity.Property(e => e.SolCurrent)
                    .HasColumnName("sol_current")
                    .HasComment("Ток соленоида");

                entity.Property(e => e.SolType)
                    .HasColumnName("sol_type")
                    .HasComment("Тип соленоида: 1-пульсирующий, 2-знакопеременный, 3-постоянный");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasDefaultValueSql("1")
                    .HasComment("предположительно\n0 - металл в работе\n1 - работа окончена\n2 - строка загружена в шину\n3 - строка обработана получателем");
            });

            modelBuilder.Entity<MeltPostgres>(entity =>
            {
                entity.ToTable("melts");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Идентификатор записи");

                entity.Property(e => e.Alg)
                    .HasMaxLength(50)
                    .HasColumnName("alg")
                    .HasComment("Номер алгоритма");

                entity.Property(e => e.Del)
                    .HasColumnName("del")
                    .HasComment("Диаметр расходуемого электрода");

                entity.Property(e => e.Diam)
                    .HasColumnName("diam")
                    .HasComment("Диаметр слитка");

                entity.Property(e => e.Dpl)
                    .HasColumnName("dpl")
                    .HasComment("Дата плавки");

                entity.Property(e => e.Gmp)
                    .HasColumnName("gmp")
                    .HasComment("Признак ГМП");

                entity.Property(e => e.Ind)
                    .HasMaxLength(50)
                    .HasColumnName("ind")
                    .HasComment("Индекс");

                entity.Property(e => e.Kont)
                    .HasMaxLength(50)
                    .HasColumnName("kont")
                    .HasComment("Контракт");

                entity.Property(e => e.LastPr)
                    .HasColumnName("last_pr")
                    .HasComment("Признак окончательного переплава");

                entity.Property(e => e.Nazn)
                    .HasMaxLength(50)
                    .HasColumnName("nazn")
                    .HasComment("Назначение");

                entity.Property(e => e.Nkompl)
                    .HasColumnName("nkompl")
                    .HasComment("Номер комплекта");

                entity.Property(e => e.NomInsp)
                    .HasMaxLength(50)
                    .HasColumnName("nom_insp")
                    .HasComment("ИЛ/УиС/ШН");

                entity.Property(e => e.Npch)
                    .HasMaxLength(2)
                    .HasColumnName("npch")
                    .HasComment("Номер печи");

                entity.Property(e => e.Nplav)
                    .HasMaxLength(6)
                    .HasColumnName("nplav")
                    .HasComment("Номер плавки");

                entity.Property(e => e.Npr)
                    .HasColumnName("npr")
                    .HasComment("Номер переплава");

                entity.Property(e => e.Ntek)
                    .HasMaxLength(20)
                    .HasColumnName("ntek")
                    .HasComment("Номер ТЭК");

                entity.Property(e => e.Pnplav)
                    .HasMaxLength(11)
                    .HasColumnName("pnplav")
                    .HasComment("Полный номер плавки");

                entity.Property(e => e.Pos)
                    .HasMaxLength(5)
                    .HasColumnName("pos")
                    .HasComment("Позиция");

                entity.Property(e => e.Pril)
                    .HasMaxLength(3)
                    .HasColumnName("pril")
                    .HasComment("Приложение");

                entity.Property(e => e.Spl)
                    .HasMaxLength(100)
                    .HasColumnName("spl")
                    .HasComment("Сплав");

                entity.Property(e => e.TabNPl)
                    .HasMaxLength(6)
                    .HasColumnName("tab_n_pl")
                    .HasComment("Табельный номер плавильщика");

                entity.Property(e => e.TokPl)
                    .HasColumnName("tok_pl")
                    .HasComment("Сила тока основного режима");
            });

            modelBuilder.Entity<SprGlobalId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("spr_global_id");

                entity.Property(e => e.GlobalId).HasColumnName("global_id");

                entity.Property(e => e.Npch)
                    .HasMaxLength(2)
                    .HasColumnName("npch");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
