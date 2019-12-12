using System;
using System.Data.Entity;

namespace Geolocation_Portal_Test.Models
{
    public class User
    {
        public int id { get; set; }
        public int role_id { get; set; }
        public int department_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime last_password_change { get; set; }
        public DateTime create_date { get; set; }
        public bool account_active { get; set; }
        public int login_attempts { get; set; }
        public DateTime last_login { get; set; }
    }

    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}