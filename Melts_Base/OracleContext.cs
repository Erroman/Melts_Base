using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;

namespace Melts_Base
{
    internal class OracleContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseOracle(@"User Id=blog;Password=<password>;Data Source=pdborcl;");
            optionsBuilder.UseOracle(@"User Id=blog;Password = dotchka1SS;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME= pdb1.mshome.net)))"); //for work computer
        }
    }
}
