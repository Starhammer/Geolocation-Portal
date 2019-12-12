using System;
using System.Data.Entity;

namespace Geolocation_Portal_Test.Models
{
    public class User
    {
        
    }

    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}