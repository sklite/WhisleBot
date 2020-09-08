using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WhisleBotConsole.DB
{
    public class UserPreference
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public long GroupId { get; set; }
        public string GroupName { get; set; }
        public string Keyword { get; set; }
    }
}
