using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LinqInEntityFramework
{
    public class CarDB : DbContext
    {
        // class represent database for Entity framework to access

        public DbSet<Car> Cars { get; set; } // a table name Cars this use for Linq

    }
}
