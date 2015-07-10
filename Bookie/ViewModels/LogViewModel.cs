using System;

namespace Bookie.ViewModels
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Core.Domains;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Forms;
    using System.Windows.Input;

    public class LogViewModel : NotifyBase
    {
        private ICommand _deleteLogsCommand;

        public ICommand DeleteLogsCommand
        {
            get
            {
                if (_deleteLogsCommand == null)
                {
                    _deleteLogsCommand = new RelayCommand(p => DeleteAllLogs(), p => _allLogEntries != null && _allLogEntries.Count > 0);
                }
                return _deleteLogsCommand;
            }
        }

        private readonly LogDomain _logDomain;

        private bool _filterError;

        public bool FilterError
        {
            get
            {
                return _filterError;
            }
            set
            {
                _filterError = value;
                NotifyPropertyChanged("FilterError");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        private DateTime? _filterDate;

        public DateTime? FilterDate
        {
            get
            {
                return _filterDate;
            }
            set
            {
                _filterDate = value;
                NotifyPropertyChanged("FilterDate");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        private bool _filterNone;

        public bool FilterNone
        {
            get
            {
                return _filterNone;
            }
            set
            {
                _filterNone = value;
                NotifyPropertyChanged("FilterNone");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        private bool _filterDebug;

        public bool FilterDebug
        {
            get
            {
                return _filterDebug;
            }
            set
            {
                _filterDebug = value;
                NotifyPropertyChanged("FilterDebug");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        private bool _filterFatal;

        public bool FilterFatal
        {
            get
            {
                return _filterFatal;
            }
            set
            {
                _filterFatal = value;
                NotifyPropertyChanged("FilterFatal");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        private bool _filterInfo;

        public bool FilterInfo
        {
            get
            {
                return _filterInfo;
            }
            set
            {
                _filterInfo = value;
                NotifyPropertyChanged("FilterInfo");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        private ObservableCollection<LogEntity> _allLogEntries;

        private ICollectionView _log;

        public ICollectionView Log
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
                NotifyPropertyChanged("Log");
            }
        }

        public LogViewModel()
        {
            _logDomain = new LogDomain();
        }

        public async void RefreshLog()
        {
            var le = await _logDomain.GetAllAsync();


            _allLogEntries = new ObservableCollection<LogEntity>(
                le);
            Log = CollectionViewSource.GetDefaultView(_allLogEntries);
            FilterDate = null;
            FilterNone = true;
        }

        private bool ApplyFilter(object item)
        {
            var log = item as LogEntity;

            if (FilterError)
            {
                if (FilterDate == null)
                {
                    return log != null && log.Level.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) >= 0;
                }
                return log != null && log.Level.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) >= 0
                       && log.Date.Date == FilterDate;
            }
            if (FilterInfo)
            {
                if (FilterDate == null)
                {
                    return log != null && log.Level.IndexOf("INFO", StringComparison.OrdinalIgnoreCase) >= 0;
                }
                return log != null && log.Level.IndexOf("INFO", StringComparison.OrdinalIgnoreCase) >= 0 &&
                log.Date.Date == FilterDate;
            }
            if (FilterDebug)
            {
                if (FilterDate == null)
                {
                    return log != null && log.Level.IndexOf("DEBUG", StringComparison.OrdinalIgnoreCase) >= 0;
                }
                return log != null && log.Level.IndexOf("DEBUG", StringComparison.OrdinalIgnoreCase) >= 0
                       && log.Date.Date == FilterDate;
            }
            if (FilterFatal)
            {
                if (FilterDate == null)
                {
                    return log != null && log.Level.IndexOf("FATAL", StringComparison.OrdinalIgnoreCase) >= 0;
                }
                return log != null && log.Level.IndexOf("FATAL", StringComparison.OrdinalIgnoreCase) >= 0
                       && log.Date.Date == FilterDate;
            }
            if (FilterNone)
            {
                if (FilterDate == null)
                {
                    return true;
                }
                return log.Date.Date == FilterDate;
            }

            return true;
        }

        public void DeleteAllLogs()
        {
            DialogResult result = MessageBox.Show("Are you sure you wish to delete the log file?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                _logDomain.RemoveAllEntrys();
                RefreshLog();
            }
        }
    }
}