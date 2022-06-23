using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melts_Base.SQLiteModels
{
    public class Melt
    {
        public int MeltId { get; set; }
        public string MeltNumber { get; set; }
        public DateTime? MeltDate { get; set; }
        public string AlloyName { get; set; }
        public string AlloyIndex { get; set; }
        public short? MouldSet { get; set; }  //не найден
        public short? ElectrodeDiameter { get; set; } //не найден
        public string MelterNumber { get; set; } //не найден
        public string MelterName { get; set; }   //не найден
        public string TEKNumber { get; set; }   
        public string IL_UiS_SHN { get; set; }
        public string Contract { get; set; }
        public string Supplement { get; set; }
        public string Purpose { get; set; }
        public short? IngotDiameter { get; set; }


        public string Npech { get; set; }
        public string Nplav { get; set; }
        public string Npart { get; set; }
        public string RazmPasp { get; set; }
        public string Splav { get; set; }
        public string Ins { get; set; }
        public string Tek { get; set; }
        public string Pereplav { get; set; }
        public string OkonchPereplav { get; set; }
        public DateTime DateZap { get; set; }
        public DateTime? DateClose { get; set; }
        public decimal? SumVesZapusk { get; set; }
        public string? Zapusk31 { get; set; }
        public string? ZapuskNakl { get; set; }
        public string? ZapuskPpf { get; set; }
        public DateTime? Dsd { get; set; }
        public string? Ncp { get; set; }
        public decimal? VesSdch { get; set; }
        public string? RazmSdch { get; set; }
        public string? MfgOrderId { get; set; }
        public string? DemandOrderId { get; set; }
        public string? Poz { get; set; }
        public string? PozNaim { get; set; }
        public string? PozRazm { get; set; }
        public string? PozIl { get; set; }
        public int MyHashCode()
        {
            return Npech?.GetHashCode() ?? 0 + Nplav?.GetHashCode() ?? 0 +
               Npart?.GetHashCode() ?? 0 + RazmPasp?.GetHashCode() ?? 0 + Splav?.GetHashCode() ?? 0 +
               Ins?.GetHashCode() ?? 0 + Tek?.GetHashCode() ?? 0 + Pereplav?.GetHashCode() ?? 0 +
               OkonchPereplav?.GetHashCode() ?? 0 + DateZap.GetHashCode() + DateClose.GetHashCode() +
               SumVesZapusk?.GetHashCode() ?? 0 + Zapusk31?.GetHashCode() ?? 0 + ZapuskNakl?.GetHashCode() ?? 0 +
               ZapuskPpf?.GetHashCode() ?? 0 + Dsd.GetHashCode() + Ncp?.GetHashCode() ?? 0 +
               VesSdch?.GetHashCode() ?? 0 + RazmSdch?.GetHashCode() ?? 0 + MfgOrderId?.GetHashCode() ?? 0 +
               DemandOrderId?.GetHashCode() ?? 0 + Poz?.GetHashCode() ?? 0 + PozNaim?.GetHashCode() ?? 0 +
               PozRazm?.GetHashCode() ?? 0 * PozIl?.GetHashCode() ?? 0;
        }
    }
}
