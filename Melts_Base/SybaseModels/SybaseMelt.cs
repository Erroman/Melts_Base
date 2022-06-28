﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melts_Base.SybaseModels
{
    public class SybaseMelt
    {
        public string? eq_id { get; set; }   //номер печи
        public string? me_num { get; set; }  //номер плавки
        public DateTime me_beg { get; set; } //время начала плавки
        public DateTime? me_end { get; set; } //время конца плавки
        public string? me_splav { get; set; } //наменование сплава
        public string? sp_name { get; set; }  //полное наименование сплава
        public string? me_mould { get;set; } //предположительно номер комплекта
        public string? me_del { get; set; } //предположительно диаметр электрода  
        public string? me_ukaz { get; set; }   //Указание
        public string? me_kont { get;set; }    //предположительно контракт
        //Key = "me_kont"
        public string? me_pril { get; set; }    //возможно приложение
        public string? me_nazn { get; set; }    //назначение
        public string? me_diam { get; set; }    //предположительно диаметр слитка
        public string? me_weigth { get; set; } //предположительно вес слитка
        public string? me_zakaz { get; set; } // ?
        public string? me_pos { get; set; }   // ?
        public string? me_kat { get; set; }   // ?
        public string? sp_id { get; set; }   // ?
        public string? me_energy { get; set; }   // ?


    }
}
