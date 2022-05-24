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
    }
}
