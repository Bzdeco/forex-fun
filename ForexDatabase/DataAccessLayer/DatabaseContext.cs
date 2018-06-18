using Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Web;

namespace ForexDatabase.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public DatabaseContext() : base("DatabaseContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}