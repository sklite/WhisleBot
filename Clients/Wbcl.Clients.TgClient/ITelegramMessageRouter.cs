using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Wbcl.Clients.TgClient
{
    public interface ITelegramMessageRouter
    {
        Task ProcessMessageAsync(Message inputMessage);
    }
}
