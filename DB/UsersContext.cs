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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var absolutePath = GetDbFilePath();
            optionsBuilder.UseSqlite($"Data Source={absolutePath}");
            LogManager.GetCurrentClassLogger().Info($"Database file address: {absolutePath}");
        }

        public static string GetDbFilePath()
        {
            string dbFileName = "users.db";
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(currentPath, dbFileName);
        }
    }
}
