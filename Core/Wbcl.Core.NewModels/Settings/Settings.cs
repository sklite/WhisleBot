namespace Wbcl.Core.Models.Settings
{
    public class Settings
    {
        public TelegramSettings Telegram { get; set; }
        public VkSettings Vkontakte { get; set; }
        public DbSettings DbSettings { get; set; }
        public WebSettings WebSettings { get; set; }
    }
}
