using Database.Models;
using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Backend
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class LocalDbContext : DbContext
    {
        // Example table
        public DbSet<T1> T1_Table { get; set; }

        // Example table
        public DbSet<T2> T2_Table { get; set; }

        public LocalDbContext()
          : base()
        {

        }

        // Constructor to use on a DbConnection that is already opened
        public LocalDbContext(DbConnection existingConnection, bool contextOwnsConnection)
          : base(existingConnection, contextOwnsConnection)
        {

        }
    }
}
