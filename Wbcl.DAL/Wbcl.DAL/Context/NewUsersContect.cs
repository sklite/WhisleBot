using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Wbcl.Core.Models.Database;

namespace Wbcl.DAL.Context
{
    //public class NewUsersContext : DbContext//, IUsersContext
    //{
    //    //public DbSet<User> Users { get; set; }
    //    //public DbSet<UserPreference> Preferences { get; set; }
    //    //public DbSet<ChatHistoryItem> ChatHistory { get; set; }

    //    //public NewUsersContext(DbContextOptions<NewUsersContext> options)
    //    //    : base(options)
    //    //{

    //    //}

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseNpgsql("Host=localhost;Database=znauDb;Username=ZnauUser;Password=heroes");
    //    }

    //    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    //{
    //    //    // _ = optionsBuilder.UseNpgsql("Host=localhost;Database=znauDb;Username=ZnauUser;Password=heroes");
    //    //    //var absolutePath = GetDbFilePath();
    //    //    //optionsBuilder.UseSqlite($"Data Source={absolutePath}");
    //    //    //LogManager.GetCurrentClassLogger().Info($"Database file address: {absolutePath}");
    //    //}

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.HasDefaultSchema("monitor");
    //        base.OnModelCreating(modelBuilder);
    //    }

    //    ////protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    ////{
    //    ////    modelBuilder.HasDefaultSchema("public");
    //    ////    base.OnModelCreating(modelBuilder);
    //    ////}

    //    //public static string GetDbFilePath()
    //    //{
    //    //    string dbFileName = "users.db";
    //    //    string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    //    //    return Path.Combine(currentPath, dbFileName);
    //    //}

    //    ////void IUsersContext.SaveChanges()
    //    ////{
    //    ////    this.SaveChanges
    //    ////}
    //}
}
