using System;
using System.Collections.Generic;

namespace Melts_Base.OracleModels
{
//MELT_ID NOT NULL NUMBER
//MELTNUMBER NOT NULL VARCHAR2(50)
//MELTDATE NOT NULL DATE
//ALLOYNAME VARCHAR2(50)
//ALLOYINDEX VARCHAR2(50)
//MOULDSET VARCHAR2(50)
//ELECTRODEDIAMETER NUMBER(38)
//MELTERNUMBER VARCHAR2(50)
//MELTERNAME VARCHAR2(50)
//TEKNUMBER VARCHAR2(50)
//IL_UIS_SHN VARCHAR2(50)
//CONTRACT VARCHAR2(50)
//SUPPLEMENT VARCHAR2(50)
//PURPOSE VARCHAR2(50)
//INGOTDIAMETER VARCHAR2(50)
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
//PASPORT_ID NOT NULL NUMBER(10)
//NPECH VARCHAR2(2)
//NPLAV VARCHAR2(15)
//NPART VARCHAR2(10)
//RAZM_PASP VARCHAR2(4000)
//SPLAV VARCHAR2(80)
//INS VARCHAR2(30)
//TEK VARCHAR2(20)
//PEREPLAV VARCHAR2(1)
//OKONCH_PEREPLAV VARCHAR2(1)
//DATE_ZAP NOT NULL DATE
//DATE_CLOSE DATE
//SUM_VES_ZAPUSK NUMBER
//ZAPUSK_31 VARCHAR2(4000)
//ZAPUSK_NAKL VARCHAR2(4000)
//ZAPUSK_PPF VARCHAR2(44)
//DSD DATE
//NCP VARCHAR2(2)
//VES_SDCH NUMBER(10,2)
//RAZM_SDCH VARCHAR2(4000)
//MFG_ORDER_ID VARCHAR2(100)
//DEMAND_ORDER_ID VARCHAR2(100)
//POZ VARCHAR2(200)
//POZ_NAIM VARCHAR2(50)
//POZ_RAZM VARCHAR2(4000)
//POZ_IL VARCHAR2(4000)
}
