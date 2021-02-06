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
        private readonly ITelegramService _tgService;
        private readonly IVkService _vkService;
        private readonly Logger _logger;

        public BotController(IOptions<Settings> settings, ITelegramService tgService, IVkService vkService)
        {
            _settings = settings.Value;
            _tgService = tgService;
            _vkService = vkService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            _tgService.Start();
            _vkService.Start();
        }

        public void Stop()
        {
            _tgService.Stop();
            _vkService.Stop();
        }
    }
}
