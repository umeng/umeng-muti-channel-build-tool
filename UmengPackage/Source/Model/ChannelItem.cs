using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UmengPackage.Source.Model
{
    [Serializable]
    public sealed class ChannelItem : INotifyPropertyChanged
    {
        //"/UmengPackage;component/Images/icon.png"
        private string channelIcon;
        public String ChannelIcon
        {
            get { return channelIcon; }
            set
            {
                if (value.StartsWith("/UmengPackage;component/"))
                {
                    channelIcon = value;
                }
                else
                {

                    channelIcon = string.Format("/UmengPackage;component/{0}", value);
                }
                NotifyPropertyChanged("ChannelIcon");
            }
        }
        private string editorImage;
        public String EditorImage
        {
            get
            {
                return editorImage;
            }
            set
            {
                if (value.StartsWith("/UmengPackage;component/"))
                {
                    editorImage = value;
                }
                else
                {
                    editorImage = string.Format("/UmengPackage;component/{0}", value);
                }
                NotifyPropertyChanged("EditorImage");
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
                channelName = value;
                NotifyPropertyChanged("ChannelName");
            }
        }

        public ChannelItem(string channelImage, string stateImage, string channelName)
        {
            ChannelIcon = channelImage;
            EditorImage = stateImage;
            ChannelName = channelName;
        }

        public ChannelItem()
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
}
