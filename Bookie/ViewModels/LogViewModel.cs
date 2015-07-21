namespace Bookie.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Forms;
    using System.Windows.Input;
    using Common;
    using Common.Model;
    using Core.Domains;
    using MessageBox = System.Windows.Forms.MessageBox;

    public class LogViewModel : NotifyBase
    {
        private readonly LogDomain _logDomain;
        private ObservableCollection<LogEntity> _allLogEntries;
        private ICommand _deleteLogsCommand;
        private DateTime? _filterDate;
        private bool _filterDebug;
        private bool _filterError;
        private bool _filterFatal;
        private bool _filterInfo;
        private bool _filterNone;
        private ICollectionView _log;


        private bool _debugMode;

        public bool DebugMode
        {
            get { return Globals.DebugMode; }
   }

        public LogViewModel()
        {
            _logDomain = new LogDomain();
        }

        public ICommand DeleteLogsCommand
        {
            get
            {
                if (_deleteLogsCommand == null)
                {
                    _deleteLogsCommand = new RelayCommand(p => DeleteAllLogs(),
                        p => _allLogEntries != null && _allLogEntries.Count > 0);
                }
                return _deleteLogsCommand;
            }
        }

        public bool FilterError
        {
            get { return _filterError; }
            set
            {
                _filterError = value;
                NotifyPropertyChanged("FilterError");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        public DateTime? FilterDate
        {
            get { return _filterDate; }
            set
            {
                _filterDate = value;
                NotifyPropertyChanged("FilterDate");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        public bool FilterNone
        {
            get { return _filterNone; }
            set
            {
                _filterNone = value;
                NotifyPropertyChanged("FilterNone");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        public bool FilterDebug
        {
            get { return _filterDebug; }
            set
            {
                _filterDebug = value;
                NotifyPropertyChanged("FilterDebug");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        public bool FilterFatal
        {
            get { return _filterFatal; }
            set
            {
                _filterFatal = value;
                NotifyPropertyChanged("FilterFatal");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        public bool FilterInfo
        {
            get { return _filterInfo; }
            set
            {
                _filterInfo = value;
                NotifyPropertyChanged("FilterInfo");
                Log.Filter = ApplyFilter;
                Log.Refresh();
            }
        }

        public ICollectionView Log
        {
            get { return _log; }
            set
            {
                _log = value;
                NotifyPropertyChanged("Log");
            }
        }

        public async void RefreshLog()
        {
            var le = await _logDomain.GetAllAsync();

            _allLogEntries = new ObservableCollection<LogEntity>(le);
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
            var result = MessageBox.Show("Are you sure you wish to delete the log file?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                _logDomain.RemoveAllEntrys();
                RefreshLog();
            }
        }
    }
}