using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UmengPackage.Source.Model
{
    [Serializable]
    public sealed class EditItem : INotifyPropertyChanged
    {
        //"/UmengPackage;component/Images/icon.png"
        private ItemState state;
        public ItemState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        private string channelName;
        public String ChannelName
        {
            get
            {
                return channelName;
            }
            set
            {
                if (channelName != value)
                {
                    channelName = value;
                    NotifyPropertyChanged("ChannelName");
                }
            }
        }

        public EditItem(ItemState state, string channelName)
        {
            State = state;
            ChannelName = channelName;
        }

        public EditItem()
        {
            // TODO: Complete member initialization
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {

            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(info));

            }

        }
    }

    public enum ItemState
    {
        Normal,
        InEdit
    }
}
