using System.Threading.Tasks;
using Wbcl.Core.Models.Notifications;

namespace Wbcl.Core.Models.Services
{
    public interface IMessageSender
    {
        Task SendMessageToUser(IMessage message);
    }
}
