// http://jesseliberty.com/2012/06/28/c-5making-inotifypropertychanged-easier/

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dsu.Common.Utilities
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
