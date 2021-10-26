using Microsoft.EntityFrameworkCore;
using System;
using Wbcl.Core.Models.Database;

namespace Wbcl.DAL.Context
{
    public interface IUsersContext : IDisposable
    {
        public DbSet<UserPreference> Preferences { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatHistoryItem> ChatHistory { get; set; }
        public int SaveChanges();
        public void Migrate();
    }
}