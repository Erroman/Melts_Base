using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melts_Base.SQLiteModels
{
    internal class Mappedmelt
    {

        public int PasportId { get; set; }
        public string? Npech { get; set; }
        public string? Nplav { get; set; }
        public string? Npart { get; set; }
        public string? RazmPasp { get; set; }
        public string? Splav { get; set; }
        public string? Ins { get; set; }
        public string? Tek { get; set; }
        public string? Pereplav { get; set; }
        public string? OkonchPereplav { get; set; }
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

    }
}
