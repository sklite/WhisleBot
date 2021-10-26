using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Wbcl.Core.Models.Database;

namespace Wbcl.DAL.Context
{
    public class UsersContext : DbContext, IUsersContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPreference> Preferences { get; set; }
        public DbSet<ChatHistoryItem> ChatHistory { get; set; }

        public void Migrate()
        {
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=znauDb;Username=ZnauUser;Password=heroes;Include Error Detail=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("monitor");
            base.OnModelCreating(modelBuilder);
        }
    }
}
