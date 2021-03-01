using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wbcl.Core.Models.Database
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public ChatState State
        {
            get;
            set;
        }
        public long? CurrentTargetId { get; set; }
        public string CurrentTargetName { get; set; }
        public PreferenceType? CurrentTargetType { get; set; }
        public string Keyword { get; set; }
        public UserType SubscriptionStatus { get; set; }
        public DateTime EndOfAdvancedSubscription { get; set; }
    }
}
