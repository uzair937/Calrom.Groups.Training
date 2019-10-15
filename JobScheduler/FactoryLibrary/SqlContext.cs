using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using JobLibrary;

namespace FactoryLibrary
{
    public class SqlContext : DbContext
    {
        public DbSet<Job> JobDb { get; set; }
        public DbSet<EmailSubscription> EmailDb { get; set; }
    }
}
