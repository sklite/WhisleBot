using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wbcl.Core.Models.Database
{
    [Table("MessagesLog")]
    public class ChatHistoryItem
    {
        [Key]
        public long Id { get; set; }
        public bool ToUser { get; set; }
        public User User { get; set; }
        public string MessageText { get; set; }
        public DateTime Sent { get; set; }
    }
}
