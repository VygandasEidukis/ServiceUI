using PropertyChanged;
using System.ComponentModel;

namespace EPS.Administration.ServiceUI.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

    }
}