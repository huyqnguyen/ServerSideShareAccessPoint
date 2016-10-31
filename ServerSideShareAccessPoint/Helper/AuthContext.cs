using Microsoft.AspNet.Identity.EntityFramework;
using ServerSideShareAccessPoint.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ServerSideShareAccessPoint.Helper
{
    public class AuthContext : IdentityDbContext<Account>
    {
        public AuthContext()
            : base("AuthContext")
        {

        }
        public DbSet<AccountInfo> AccountInfos { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}