using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WhisleBotConsole.DB
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long ChatId { get; set; }
        public ChatState State { get; set; }
        public long? CurrentGroupId { get; set; }
        public string CurrentGroupName { get; set; }
        public string Keyword { get; set; }

    }
}
