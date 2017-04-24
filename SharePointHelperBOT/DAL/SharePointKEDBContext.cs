using SharePointHelperBOT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SharePointHelperBOT.DAL
{
    [Serializable]
    public class SharePointKEDBContext : DbContext
    {
        public SharePointKEDBContext() : base("SharePointKEDBContext")
        {

        }

        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}