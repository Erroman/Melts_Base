using System;
using System.Collections.Generic;

namespace Melts_Base.PostgresFiles
{
    public partial class Epasport
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int AsutpPaspId { get; set; }
        /// <summary>
        /// ID оборудования GLOBAL
        /// </summary>
        public long? GlobalId { get; set; }
        /// <summary>
        /// Номер плавки
        /// </summary>
        public string MeltNumber { get; set; }
        /// <summary>
        /// Тип операции: 1-приварка, 3-плавка, 5-оплавление
        /// </summary>
        public int OperType { get; set; }
        /// <summary>
        /// Признак окончания операции
        /// </summary>
        public bool? OperFinished { get; set; }
        /// <summary>
        /// Время начала проверки натекания
        /// </summary>
        public DateTime? LtStartTime { get; set; }
        /// <summary>
        /// Время окончания проверки натекания
        /// </summary>
        public DateTime? LtStopTime { get; set; }
        /// <summary>
        /// Остаточное давление в начале проверки натекания
        /// </summary>
        public double? LtStartPres { get; set; }
        /// <summary>
        /// Остаточное давление в конце проверки натекания
        /// </summary>
        public double? LtStopPres { get; set; }
        /// <summary>
        /// Фактическое натекание
        /// </summary>
        public double? LtLeak { get; set; }
        /// <summary>
        /// Время начала охлаждения
        /// </summary>
        public DateTime? CoolingStartTime { get; set; }
        /// <summary>
        /// Время окончания охлаждения
        /// </summary>
        public DateTime? CoolingStopTime { get; set; }
        /// <summary>
        /// Фактическое время охлаждения
        /// </summary>
        public TimeSpan? CoolingTotalTime { get; set; }
        /// <summary>
        /// Средя в которой происходит охлаждение: 1-гелий, 2-аргон
        /// </summary>
        public int? CoolingEnv { get; set; }
        /// <summary>
        /// Время начала плавления
        /// </summary>
        public DateTime? MeltStartTime { get; set; }
        /// <summary>
        /// Остаточное давление факт, мм рт.ст.
        /// </summary>
        public double? MeltPressure { get; set; }
        /// <summary>
        /// Фактическое время плавки
        /// </summary>
        public TimeSpan? MeltTotalTime { get; set; }
        /// <summary>
        /// Ток дуги
        /// </summary>
        public double? ArcCurrent { get; set; }
        /// <summary>
        /// Напряжение дуги
        /// </summary>
        public double? ArcVoltage { get; set; }
        /// <summary>
        /// Диапазон напряжения минимум
        /// </summary>
        public double? ArcVoltageMin { get; set; }
        /// <summary>
        /// Диапазон напряжения максимум
        /// </summary>
        public double? ArcVoltageMax { get; set; }
        /// <summary>
        /// Ток соленоида
        /// </summary>
        public double? SolCurrent { get; set; }
        /// <summary>
        /// Тип соленоида: 1-пульсирующий, 2-знакопеременный, 3-постоянный
        /// </summary>
        public int? SolType { get; set; }
        /// <summary>
        /// предположительно
        /// 0 - металл в работе
        /// 1 - работа окончена
        /// 2 - строка загружена в шину
        /// 3 - строка обработана получателем
        /// </summary>
        public int? State { get; set; }
    }
}
