namespace Bookie.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common;
    using Common.Model;
    using Core.Domains;

    public class SettingsViewModel : NotifyBase
    {
        private ICommand _deleteExcludedCommand;
        private ObservableCollection<Excluded> _excluded;

        public SettingsViewModel()
        {
            var all = new ExcludedDomain().GetAllExcluded();
            Excluded = new ObservableCollection<Excluded>(all);
        }

        public ICommand DeleteExcludedCommand
        {
            get
            {
                return _deleteExcludedCommand
                       ?? (_deleteExcludedCommand = new RelayCommand(DeleteExcluded, p => true));
            }
        }

        public ObservableCollection<Excluded> Excluded
        {
            get { return _excluded; }
            set
            {
                _excluded = value;
                NotifyPropertyChanged("Excluded");
            }
        }

        private void DeleteExcluded(object parameter)
        {
            var s = (Excluded) parameter;
        }
    }
}