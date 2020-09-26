using BotServer.TelegramBot;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WhisleBotConsole.Config;
using WhisleBotConsole.Vk;

namespace WhisleBotConsole
{
    class BotController : IBotController
    {
        private readonly Settings _settings;
        private readonly ITelegramService _botService;
        private readonly IVkGroupsSearcher _vkGroupSearcher;
        private readonly Logger _logger;

        public BotController(IOptions<Settings> settings, ITelegramService botService, IVkGroupsSearcher groupSearcher)
        {
            _settings = settings.Value;
            _botService = botService;
            _vkGroupSearcher = groupSearcher;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            _botService.Start();
            _logger.Info($"Bot started");

            _vkGroupSearcher.StartSearch(_settings.Vkontakte.BaseSearchInterval);
        }

        public void Stop()
        {
            _botService.Stop();
            _logger.Info($"Bot stopped");

            _vkGroupSearcher.StopSearch();
        }
    }
}
