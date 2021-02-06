using System;
using System.ComponentModel.DataAnnotations;

namespace WhisleBotConsole.DB
{
    public class UserPreference
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public long TargetId { get; set; }
        public string TargetName { get; set; }
        public PreferenceType TargetType { get; set; }
        public string Keyword { get; set; }
        public DateTime LastNotifiedPostTime { get; set; }
    }
}
