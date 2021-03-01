using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Timers;
using VkNet;
using Wbcl.Core.Models.Services;
using Wbcl.Core.Models.Settings;

namespace WhisleBotConsole.Vk
{
    public class VkService : IMonitorService
    {
        private Timer _timer;
        private bool _isSearching = false;
        private readonly IVkGroupsCrawler _groupSearcher;
        private readonly Settings _settings;
        private readonly Logger _logger;


        public VkService(IVkGroupsCrawler groupSearcher, Settings settings)
        {
            _groupSearcher = groupSearcher;
            _settings = settings;
            _logger = LogManager.GetCurrentClassLogger();
        }

        private void SetTimer(int seconds)
        {
            _timer = new Timer(seconds * 1000);
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
                _logger.Info("Finished searching mentions");

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception occured in VkService. {ex.ToString()}");
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
