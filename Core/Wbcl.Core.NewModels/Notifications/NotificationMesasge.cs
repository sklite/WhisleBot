using System;
using System.Collections.Generic;
using System.Text;

namespace Wbcl.Core.Models.Notifications
{
    public class NotificationMesasge : IMessage
    {
        public long ChatId { get; set; }
        public string Text { get; set; }
    }
}
