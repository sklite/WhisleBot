using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using VkNet;
using WhisleBotConsole.Config;

namespace WhisleBotConsole.Vk
{
    class VkService : IVkService
    {
        private Timer _timer;
        private bool _isSearching = false;
        private readonly IVkGroupsCrawler _groupSearcher;
        private readonly Settings _settings;
        private readonly VkApi _api;
        private readonly Logger _logger;


        public VkService(IVkGroupsCrawler groupSearcher, IOptions<Settings> settings)
        {
            _groupSearcher = groupSearcher;
            _settings = settings.Value;
            _logger = LogManager.GetCurrentClassLogger();
        }

        private void SetTimer(int milliseconds)
        {
            // Create a timer with a two second interval.
            _timer = new Timer(milliseconds);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //Avoid multiple vk calls at the same time
            if (_isSearching)
                return;
            try
            {
                _isSearching = true;
                _logger.Info("Searching mentions...");
                _groupSearcher.DoSearch();
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception occured in VkService.");
            }
            finally
            {
                _isSearching = false;
            }

        }

        public void Start()
        {
            SetTimer(_settings.Vkontakte.BaseSearchInterval);
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
