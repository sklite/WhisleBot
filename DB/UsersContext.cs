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
            // Gets the current path (executing assembly)
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Your DB filename    
            string dbFileName = "users.db";
            // Creates a full path that contains your DB file
            string absolutePath = Path.Combine(currentPath, dbFileName);

            optionsBuilder.UseSqlite($"Data Source={absolutePath}");
            LogManager.GetCurrentClassLogger().Info($"Database file address: {absolutePath}");

        }
    }
}
