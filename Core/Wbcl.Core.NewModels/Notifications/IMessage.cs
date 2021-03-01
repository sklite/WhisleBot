using System;
using System.Collections.Generic;
using System.Text;
using Wbcl.Core.Models.Database;

namespace Wbcl.Core.Models.Notifications
{
    public interface IMessage
    {
        public long ChatId { get; set; }
        //public UserPreference Preference { get; set; }
        public string Text { get; set; }
    }
}
