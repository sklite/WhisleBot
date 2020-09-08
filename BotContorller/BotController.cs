using BotServer.TelegramBot;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using WhisleBotConsole.Config;

namespace WhisleBotConsole
{
    class BotController : IBotController
    {
        private readonly IOptions<Settings> _settings;
        private readonly ITelegramService _botService;
        private readonly Logger _logger;

        public BotController(IOptions<Settings> settings, ITelegramService botService)
        {
            _settings = settings;
            _botService = botService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            _botService.Start();
            _logger.Info($"Bot started");
        }

        public void Stop()
        {
            _botService.Stop();
            _logger.Info($"Bot stopped");
        }
    }
}
