using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace UmengWidget.Model
{
    public class UmengMeta : INotifyPropertyChanged
    {
        private string appkey = "";
        public string Appkey
        {
            get { return appkey; }
            set
            {
                if (value != appkey)
                {
                    appkey = value;
                    NotifyPropertyChanged("Appkey");
                }
            }
        }

        private string channel = "";
        public string Channel
        {
            get { return channel; }
            set
            {
                if (value != channel)
                {
                    channel = value;
                    NotifyPropertyChanged("Channel");
                }
            }
        }

        private string components = "";
        public string Components
        {
            get { return components; }
            set
            {
                if (value != components)
                {
                    components = value;
                    NotifyPropertyChanged("Components");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
