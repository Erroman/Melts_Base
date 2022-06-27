using System;
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
                                             
    }
}
