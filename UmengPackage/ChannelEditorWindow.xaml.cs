using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UmengPackage
{
    /// <summary>
    /// Interaction logic for ChannelEditorWindow.xaml
    /// </summary>
    public partial class ChannelEditorWindow : Window
    {
        string channel_string = null;

        public ChannelEditorWindow()
        {
            InitializeComponent();
        }

        public string getChannels()
        {
            return channel_string;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            channel_string = null;
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            channel_string = this.channels.Text;
            Close();
        }
    }
}
