﻿using System;
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
            optionsBuilder.UseOracle(@"User Id=blog;Password=<password>;Data Source=pdborcl;");
        }
    }
}
