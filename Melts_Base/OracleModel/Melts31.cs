using System;
using System.Collections.Generic;

namespace Melts_Base.OracleModel
{
    public partial class Melts31
    {
        public decimal MeltId { get; set; }
        public string Meltnumber { get; set; }
        public DateTime Meltdate { get; set; }
        public string Alloyname { get; set; }
        public string Alloyindex { get; set; }
        public string Mouldset { get; set; }
        public decimal? Electrodediameter { get; set; }
        public string Melternumber { get; set; }
        public string Meltername { get; set; }
        public string Teknumber { get; set; }
        public string IlUisShn { get; set; }
        public string Contract { get; set; }
        public string Supplement { get; set; }
        public string Purpose { get; set; }
        public string Ingotdiameter { get; set; }
    }
}
