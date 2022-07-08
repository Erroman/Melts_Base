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
        public string this[int me] 
        {
            get { return IndexDict[me]; }

        }
        string[] Index = 
            {

            };
        Dictionary<int,string> IndexDict = new Dictionary<int,string>();
        private int melt_id;
        public int MeltId { get => melt_id;  set { melt_id = value; IndexDict[0] = value.ToString(); }  }
        private string? eq_id;
        public string? Eq_id { get =>eq_id; set { eq_id = value; IndexDict[1] = value.ToString(); } }   //номер печи
        private string? me_num;
        public string? Me_num { get => me_num; set { me_num = value; IndexDict[2] = value.ToString(); } } //номер плавки
        private DateTime? me_beg;
        public DateTime? Me_beg { get => me_beg; set { me_beg = value; IndexDict[3] = value.ToString(); } } //время начала плавки
        private DateTime? me_end;
        public DateTime? Me_end { get => me_end; set { me_end = value; IndexDict[4] = value.ToString(); } } //время конца плавки
        private string sp_name;
        public string? Sp_name { get => sp_name; set { sp_name = value; IndexDict[5] = value.ToString(); } } //полное наименование сплава
        private string? oracle_Ins;
        public string? Oracle_Ins { get => oracle_Ins; set { oracle_Ins = value; IndexDict[6] = value.ToString(); } }//индекс сплава из Оракл
        private string? me_mould;
        public string? Me_mould { get => me_mould; set { me_mould = value; IndexDict[7] = value.ToString(); } }//предположительно номер комплекта
        private string? me_del;
        public string? Me_del { get => me_del; set { me_del = value; IndexDict[8] = value.ToString(); } }//предположительно диаметр электрода  
        private string? oracle_Tek;
        public string? Oracle_Tek { get => oracle_Tek; set { oracle_Tek = value; IndexDict[9] = value.ToString(); } }//№ТЕК из Оракл
        private string? me_ukaz;
        public string? Me_ukaz { get => me_ukaz; set { me_ukaz = value; IndexDict[10] = value.ToString(); } }//Указание
        private string? me_kont;
        public string? Me_kont { get => me_kont; set { me_kont = value; IndexDict[11] = value.ToString(); } }//предположительно контракт
        //Key = "Me_kont"
        private string? me_pril;
        public string? Me_pril { get => me_pril; set { me_pril = value; IndexDict[12] = value.ToString(); } }//возможно приложение
        private string? me_nazn;
        public string? Me_nazn { get => me_nazn; set { me_nazn = value; IndexDict[13] = value.ToString(); } }//назначение
        private string? me_diam;
        public string? Me_diam { get => me_diam; set { me_diam = value; IndexDict[14] = value.ToString(); } }//предположительно диаметр слитка
        private string? me_weight;
        public string? Me_weight { get => me_weight; set { me_weight = value; } }//предположительно вес слитка
        private string? me_zakaz;
        public string? Me_zakaz { get => me_zakaz; set { me_zakaz = value;} } // ?
        private string? me_pos;
        public string? Me_pos { get => me_pos; set { me_pos = value;} }// ?
        private string? me_kat;
        public string? Me_kat { get => me_kat; set { me_kat = value;} }// ?
        private string? sp_id;
        public string? Sp_id { get => sp_id; set { sp_id = value;} }// ?
        private string? me_energy;
        public string? Me_energy { get => sp_id; set { sp_id = value;} }// ?
        private string? me_splav;
        public string? Me_splav { get => me_splav; set { me_splav = value; } } //наменование сплава
        public int MyHashCode()
        {
            return Eq_id?.GetHashCode() ?? 0 + Me_num?.GetHashCode() ?? 0 +
               Me_beg.GetHashCode() + Me_end.GetHashCode() + Me_splav?.GetHashCode() ?? 0 +
               Sp_name?.GetHashCode() ?? 0 + Me_mould?.GetHashCode() ?? 0 + Me_del?.GetHashCode() ?? 0 +
               Me_ukaz?.GetHashCode() ?? 0 + Me_kont?.GetHashCode() ?? 0 + Me_pril?.GetHashCode() ?? 0 +
               Me_nazn?.GetHashCode() ?? 0 + Me_diam?.GetHashCode() ?? 0 + Me_weight?.GetHashCode() ?? 0 +
               Me_zakaz?.GetHashCode() ?? 0 + Me_pos.GetHashCode() + Me_kat?.GetHashCode() ?? 0 +
               Sp_id?.GetHashCode() ?? 0 + Me_energy?.GetHashCode() ?? 0 +
               Oracle_Ins?.GetHashCode() ?? 0 + Oracle_Tek?.GetHashCode() ?? 0;
        }


    }
}
