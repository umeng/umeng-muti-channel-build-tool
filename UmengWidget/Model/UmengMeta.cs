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
        private string appkey = "Appkey:1231w12asdq23r2";
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

        private string channel = "Channel:GooglePlay";
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

        private Visibility analytics;
        public Visibility Analytics
        {
            get { return analytics; }
            set
            {
                if (value != analytics)
                {
                    analytics = value;
                    NotifyPropertyChanged("Analytics");
                }
            }
        }

        private Visibility update;
        public Visibility Update
        {
            get { return analytics; }
            set
            {
                if (value != update)
                {
                    update = value;
                    NotifyPropertyChanged("Update");
                }
            }
        }

        private Visibility feedback;
        public Visibility Feedback
        {
            get { return feedback; }
            set
            {
                if (value != feedback)
                {
                    feedback = value;
                    NotifyPropertyChanged("Feedback");
                }
            }
        }

        private Visibility xp;
        public Visibility XP
        {
            get { return xp; }
            set
            {
                if (value != xp)
                {
                    xp = value;
                    NotifyPropertyChanged("XP");
                }
            }
        }

        private Visibility social;
        public Visibility Social
        {
            get { return social; }
            set
            {
                if (value != social)
                {
                    social = value;
                    NotifyPropertyChanged("Social");
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
