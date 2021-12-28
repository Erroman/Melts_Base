using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melts_Base
{
    public class Melt
    {
        public int MeltId { get; set; }
        public string MeltNumber { get; set; }
        public DateOnly? MeltDate { get; set; }
        public string AlloyName { get; set; }
        public string AlloyIndex { get; set; }
        public short? MouldSet { get; set; }
        public int ElectrodeDiameter { get; set; }
        public string MelterNumber { get; set; }
        public string MelterName { get; set; }
        public string TEKNumber { get; set; }
        public string IL_UiS_SHN { get; set; }
        public string Contract { get; set; }  
        public string Supplement { get; set; }       
        public string Purpose { get; set; }
        public int IngotDiameter { get; set; }
    }
}
