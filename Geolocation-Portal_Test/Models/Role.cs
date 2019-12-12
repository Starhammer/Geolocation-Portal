using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Geolocation_Portal_Test.Models
{
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class RoleDBContext : DbContext
    {
        public DbSet<Role> Users { get; set; }
    }
}