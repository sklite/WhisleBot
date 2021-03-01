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

        //public void Migrate()
        //{
        //    this.Migrate();
        //}

        //public UsersContext(DbContextOptions<UsersContext> options)
        //    : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=znauDb;Username=ZnauUser;Password=heroes;Include Error Detail=true");
        }

        ////protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        ////{
        ////    // _ = optionsBuilder.UseNpgsql("Host=localhost;Database=znauDb;Username=ZnauUser;Password=heroes");
        ////    //var absolutePath = GetDbFilePath();
        ////    //optionsBuilder.UseSqlite($"Data Source={absolutePath}");
        ////    //LogManager.GetCurrentClassLogger().Info($"Database file address: {absolutePath}");
        ////}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("monitor");
            base.OnModelCreating(modelBuilder);
        }

        ////protected override void OnModelCreating(ModelBuilder modelBuilder)
        ////{
        ////    modelBuilder.HasDefaultSchema("public");
        ////    base.OnModelCreating(modelBuilder);
        ////}

        //public static string GetDbFilePath()
        //{
        //    string dbFileName = "users.db";
        //    string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    return Path.Combine(currentPath, dbFileName);
        //}
    }
}
