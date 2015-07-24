using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookie.ViewModels
{
    using Common;

    public class NickNameViewModel : NotifyBase
    {
        private string _nickName;

        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value;
                NotifyPropertyChanged("NickName");
            }
        }
    }
}
