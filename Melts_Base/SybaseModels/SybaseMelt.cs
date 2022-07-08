using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Melts_Base.SybaseModels
{
    public class SybaseMelt
    {
        public string? Me_id { get; set; }   //номер записи
        public string? Eq_id { get; set; }   //номер печи
        public string? Me_num { get; set; }  //номер плавки
        public DateTime Me_beg { get; set; } //время начала плавки
        public DateTime? Me_end { get; set; } //время конца плавки
        public string? Me_splav { get; set; } //наменование сплава
        public string? Sp_name { get; set; }  //полное наименование сплава
        public string? Me_mould { get;set; } //предположительно номер комплекта
        public string? Me_del { get; set; } //предположительно диаметр электрода  
        public string? Me_ukaz { get; set; }   //Указание
        public string? Me_kont { get;set; }    //предположительно контракт
        //Key = "Me_kont"
        public string? Me_pril { get; set; }    //возможно приложение
        public string? Me_nazn { get; set; }    //назначение
        public string? Me_diam { get; set; }    //предположительно диаметр слитка
        public string? Me_weight { get; set; } //предположительно вес слитка
        public string? Me_zakaz { get; set; } // ?
        public string? Me_pos { get; set; }   // ?
        public string? Me_kat { get; set; }   // ?
        public string? Sp_id { get; set; }   // ?
        public string? Me_energy { get; set; }   // ?
        public string? Oracle_Ins { get; set; } //индекс сплава из Оракл
        public string? Oracle_Tek { get; set; } //№ТЕК из Оракл
        public int MyHashCode()
        {
            return Eq_id?.GetHashCode() ?? 0 + Me_num?.GetHashCode() ?? 0 +
               Me_beg.GetHashCode() + Me_end.GetHashCode() + Me_splav?.GetHashCode() ?? 0 +
               Sp_name?.GetHashCode() ?? 0 + Me_mould?.GetHashCode() ?? 0 + Me_del?.GetHashCode() ?? 0 +
               Me_ukaz?.GetHashCode() ?? 0 + Me_kont?.GetHashCode() ?? 0 + Me_pril?.GetHashCode() ?? 0 +
               Me_nazn?.GetHashCode() ?? 0 + Me_diam?.GetHashCode() ?? 0 + Me_weight?.GetHashCode() ?? 0 +
               Me_zakaz?.GetHashCode() ?? 0 + Me_pos.GetHashCode() + Me_kat?.GetHashCode() ?? 0 +
               Sp_id?.GetHashCode() ?? 0 + Me_energy?.GetHashCode() ?? 0 +
               Oracle_Ins?.GetHashCode() ?? 0 +Oracle_Tek?.GetHashCode() ?? 0;
        }


    }
}
