namespace Bookie.ViewModels
{
    using Common;

    public class NickNameViewModel : NotifyBase
    {
        private string _nickName;

        public string NickName
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                NotifyPropertyChanged("NickName");
            }
        }
    }
}