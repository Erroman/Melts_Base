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
        public string MeltName { get; set; }
        public string AlloyName { get; set; }
        public string AlloyDescription { get; set; }
        public string Electrode { get; set; }
        public string ElectrodeDescription { get; set; }
        public string MelterNumber { get; set; }
        public string MelterName { get; set; }
        public string MelterDescription { get; set; }
        public string AssignmentNumber { get; set; }
        public string AssignmentName { get; set; }
        public string AssignmentDescription { get; set; }
        public string Directions { get; set; }
        public string DirectionsDescription { get; set; }
        public string Contract { get; set; }
        public string ContractDescription { get; set; }
        public string Purpose { get; set; }
        public string PurposeDescription { get; set; }
        public int IngotDiameter { get; set; }
    }
}
