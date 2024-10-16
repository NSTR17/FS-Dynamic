using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FS_Dynamic
{
    internal class ApplicationContext:DbContext
    {
        public DbSet<Result> Results { get; set; }
        public DbSet<Timer> Timers { get; set; }

        public ApplicationContext():base("DefaultConnection") {}
    }
}
