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
    {  //поле не входит в hash-code!
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
        public string? Oracle_Poz { get; set; } //приложение из Оракл
        public string? Oracle_PozNaim { get; set; } //назначание из Оракл
        public string? Oracle_Pereplav { get; set; } //номер переплава из Оракл
        public string? Oracle_OkonchPereplav { get; set; } //признак окончательного переплава из Оракл

        public string MyHashCode()
        {
              return (Eq_id + Me_num +
               Me_beg + Me_end + Me_splav +
               Sp_name + Me_mould + Me_del +
               Me_ukaz + Me_kont + Me_pril +
               Me_nazn + Me_diam + Me_weight +
               Me_zakaz + Me_pos + Me_kat +
               Sp_id + Me_energy +
               Oracle_Ins + Oracle_Tek +
               Oracle_Poz + Oracle_PozNaim +
               Oracle_Pereplav + Oracle_OkonchPereplav);
        }


    }
}
