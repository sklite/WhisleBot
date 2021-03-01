using NLog;
using System;
using System.Timers;
using Wbcl.Core.Models.Services;
using Wbcl.Core.Models.Settings;

namespace Wbcl.Monitors.WebMonitor
{
    public class WebMonitorService : IMonitorService
    {
        private Timer _timer;
        private bool _isSearching = false;
        private readonly Settings _settings;
        private readonly Logger _logger;


        public WebMonitorService(Settings settings)
        {
            _settings = settings;
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Info("WebMonitorService instantiated");

        }

        private void SetTimer(int seconds)
        {
            _timer = new Timer(seconds * 1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //Avoid multiple calls at the same time
            if (_isSearching)
                return;
            try
            {
                _isSearching = true;
                _logger.Info("Searching websites...");
          //      _groupSearcher.DoSearch();
                _logger.Info("Finished searching mentions");

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception occured in WebMonitorService. {ex}");
            }
            finally
            {
                _isSearching = false;
            }

        }

        public void Start()
        {
            SetTimer(_settings.WebSettings.BaseSearchInterval);
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
