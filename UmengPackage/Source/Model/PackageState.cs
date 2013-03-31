using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Data;

namespace UmengPackage.Source.Model
{
    public class PackageState : INotifyPropertyChanged
    {
        private string channelName;
        public string ChannelName 
        {
            get { return channelName; }
            set
            {
                if (channelName != value)
                {
                    channelName = value;
                    NotifyPropertyChanged("ChannelName");
                }
            }
        }

        private State currentState;
        public State CurrentState
        {
            get
            {
                return currentState;
            }
            set 
            {
                if (currentState != value)
                {
                    currentState = value;
                    NotifyPropertyChanged("CurrentState");
                }     
            } 
        }

        public PackageState()
        {
        }

        public PackageState(string channel, State state)
        {
            ChannelName = channel;
            CurrentState = state;
        }

        public PackageState setState(State state)
        {
            CurrentState = state;
            return this;
        }

        public PackageState setChannel(string channel)
        {
            ChannelName = channel;
            return this;
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

    public enum State { 
        EMPTY = 0xABABAB, START = 0xFF0000, END = 0x00FF00, FAILURE = 0x0000FF
    }

    public class StatusToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int color =  (int)((State)value);
            byte R = (byte)(color >> 11);
            byte G = (byte)(color >> 5);
            byte B = (byte)color;

            return new SolidColorBrush(Color.FromRgb(R, G, B));
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
