using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace EPS.Administration.ServiceUI.Model
{
    public class SerialNumberModel : INotifyPropertyChanged
    {
        private string _serialNumber;

        public string SerialNumber
        {
            get { return _serialNumber ?? "New device"; }
            set 
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }

        public override string ToString()
        {
            return SerialNumber ?? "New device";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
