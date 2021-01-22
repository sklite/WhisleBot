using Microsoft.EntityFrameworkCore;
using NLog;
using System.IO;
using System.Reflection;

namespace WhisleBotConsole.DB
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPreference> Preferences { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // _ = optionsBuilder.UseNpgsql("Host=localhost;Database=znauDb;Username=ZnauUser;Password=heroes");
            //var absolutePath = GetDbFilePath();
            //optionsBuilder.UseSqlite($"Data Source={absolutePath}");
            //LogManager.GetCurrentClassLogger().Info($"Database file address: {absolutePath}");
        }

        //protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasDefaultSchema("public");
        //    base.OnModelCreating(modelBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

        public static string GetDbFilePath()
        {
            string dbFileName = "users.db";
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(currentPath, dbFileName);
        }
    }
}
