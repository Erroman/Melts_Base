using System;
using System.Collections.Generic;

namespace Melts_Base.PostgresFiles
{
    public partial class MeltPostgres
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Номер печи
        /// </summary>
        public string Npch { get; set; }
        /// <summary>
        /// Номер плавки
        /// </summary>
        public string Nplav { get; set; }
        /// <summary>
        /// Полный номер плавки
        /// </summary>
        public string Pnplav { get; set; }
        /// <summary>
        /// Дата плавки
        /// </summary>
        public DateOnly? Dpl { get; set; }
        /// <summary>
        /// Сплав
        /// </summary>
        public string Spl { get; set; }
        /// <summary>
        /// Индекс
        /// </summary>
        public string Ind { get; set; }
        /// <summary>
        /// Номер комплекта
        /// </summary>
        public short? Nkompl { get; set; }
        /// <summary>
        /// Диаметр расходуемого электрода
        /// </summary>
        public short? Del { get; set; }
        /// <summary>
        /// Табельный номер плавильщика
        /// </summary>
        public string TabNPl { get; set; }
        /// <summary>
        /// Номер ТЭК
        /// </summary>
        public string Ntek { get; set; }
        /// <summary>
        /// ИЛ/УиС/ШН
        /// </summary>
        public string NomInsp { get; set; }
        /// <summary>
        /// Контракт
        /// </summary>
        public string Kont { get; set; }
        /// <summary>
        /// Приложение
        /// </summary>
        public string Pril { get; set; }
        /// <summary>
        /// Позиция
        /// </summary>
        public string Pos { get; set; }
        /// <summary>
        /// Назначение
        /// </summary>
        public string Nazn { get; set; }
        /// <summary>
        /// Диаметр слитка
        /// </summary>
        public short? Diam { get; set; }
        /// <summary>
        /// Номер алгоритма
        /// </summary>
        public string Alg { get; set; }
        /// <summary>
        /// Номер переплава
        /// </summary>
        public short? Npr { get; set; }
        /// <summary>
        /// Признак окончательного переплава
        /// </summary>
        public bool? LastPr { get; set; }
        /// <summary>
        /// Сила тока основного режима
        /// </summary>
        public float? TokPl { get; set; }
        /// <summary>
        /// Признак ГМП
        /// </summary>
        public bool? Gmp { get; set; }
    }
}
