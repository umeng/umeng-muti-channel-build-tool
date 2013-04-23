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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using UIControls;

using UmengPackage.Source.Model;
using UmengPackage.Source.Common;
using System.Windows.Controls.Primitives;

namespace UmengPackage
{
    /// <summary>
    /// Interaction logic for ChannelTemplate.xaml
    /// </summary>
    public partial class ConfigTemplate : Window, INotifyPropertyChanged
    {
        ProjectConfigration config = null;

        ObservableCollection<ChannelItem> Candinate = new ObservableCollection<ChannelItem>();
        ObservableCollection<ChannelItem> ChannelTemplate = new ObservableCollection<ChannelItem>();

        public ConfigTemplate()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        public ConfigTemplate(String configFile):this()
        {
            //bind
            DataContext = this;
            //load configration
            if (!string.IsNullOrEmpty(configFile))
            {
                config = ProjectConfigration.readSettingFromFile(configFile);

                SettingFile = configFile;

                KeystoreFilePath = config.KeystoreFilePath;
                KeyStorePw = config.KeystorePassword;
                Alias = config.Alias;
                AliasPw = config.AliasPassword;

                foreach (string item in config.Candinate)
                {
                    Candinate.Add(new EditItem(item, EditState.Normal));
                }
            }
            else
            {
                config = new ProjectConfigration();
            }

            Candinate.Add(new EditItem("", EditState.Editable));
            Channels.ItemsSource = Candinate;

            LoadTemplate();
        }

        public void LoadTemplate()
        {
            List<string> t = ProjectConfigration.GetTemplate();

            foreach( string item in t)
            {
                ChannelTemplate.Add(new TemplateItem( item , false));
            }


            var coincide = from A in ChannelTemplate
                           from B in Candinate
                           where A.ItemName.Equals(B.ItemName)
                           select A;

            foreach (ChannelItem item in coincide)
            {
                (item as TemplateItem).IsChecked = true;
            }

            this.lb_template.ItemsSource = ChannelTemplate;
        }

        private void FindToggleAndInitTemplateState(DependencyObject obj)
        {
            int N = VisualTreeHelper.GetChildrenCount(obj);

            for( int i =0; i< N; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is ToggleButton)
                {
                    String name = FindToggleName(child);

                    if (name != null)
                    {
                       // templateState.Add( name , (ToggleButton)child);
                    }
                }
                else
                {
                    FindToggleAndInitTemplateState(child as DependencyObject);
                }
            }
        }

        private String FindToggleName(DependencyObject obj)
        {
            int M = VisualTreeHelper.GetChildrenCount(obj);

            for (int i = 0; i < M; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is TextBlock)
                {
                    return ((TextBlock)child).Text;
                }
                else
                {
                    return FindToggleName(child);
                }
            }

            return null;
        }

        private void SearchTextBox_FindKeyStore(object sender, RoutedEventArgs e)
        {
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            openFileDialog1.DefaultExt = "keystore";
            openFileDialog1.Filter = "keystore files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                KeystoreFilePath = openFileDialog1.FileName;
            }
        }

        //Save current configration
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckInput();

                config.KeystoreFilePath = keystore_file_path;
                config.KeystorePassword = KeyStorePw;
                config.Alias = Alias;
                config.AliasPassword = AliasPw;

                config.Candinate.Clear();

                foreach (EditItem item in Candinate.Where( T=> ((T as EditItem).State == EditState.Normal)))
                {
                    config.Candinate.Add(item.ItemName);
                }

                config.WriteSettintToFile(SettingFile);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Dismiss dialog
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool CheckInput()
        {
            //keystore
            if (string.IsNullOrEmpty(KeystoreFilePath))
            {
                throw new Exception("没有设置 keystore");
            }

            //keystore pw

            if (string.IsNullOrEmpty(KeyStorePw))
            {
                throw new Exception("没有设置 keystore password");
            }

            //alias

            if (string.IsNullOrEmpty(Alias))
            {
                throw new Exception("没有设置 keystore entry (alias)");
            }

            //entry pw

            if (string.IsNullOrEmpty(AliasPw))
            {
                throw new Exception("没有设置 keystore entry password");
            }

            //channel
            if (Candinate == null || Candinate.Count == 0)
            {
                throw new Exception("没有设置 渠道 ");
            }

            //setting file name

            if (string.IsNullOrEmpty(SettingFile))
            {
                throw new Exception("亲，起个名呗 ^_^ ");
            }

            return true;
        }

        private void UTextBox_ItemDelete(object sender, RoutedEventArgs e)
        {
            var arg = e as ItemEditCompleteEventArgs;

            if (arg != null)
            {
                var channel = arg.Content;

                System.Diagnostics.Debug.WriteLine("Delete Channel : " + channel);

                //remove from candinate
                var edit = Candinate.find(channel);

                if (edit != null)
                {
                    Candinate.Remove(edit);
                }
                //detag from template

                var item = ChannelTemplate.find(channel) as TemplateItem;

                if (item != null)
                {
                    item.IsChecked = false;
                }
               
            }
        }

        private void AddChannel(String name)
        {
            var item = Candinate.find(name);

            if (item != null)
            {
                System.Diagnostics.Debug.WriteLine("Already exits");
            }
            else
            {
                Candinate.Insert(Candinate.Count - 1, new EditItem(name, EditState.Normal));
            }
        }

        private void UTextBox_ItemEditComplete(object sender, RoutedEventArgs e)
        {
            var arg = e as ItemEditCompleteEventArgs;

            if (arg != null)
            {
                AddChannel(arg.Content );
                
                //tag in template
                var _item = ChannelTemplate.find(arg.Content) as TemplateItem;
                if (_item != null)
                {
                    _item.IsChecked = true;
                }
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;

            var enumerator =  LogicalTreeHelper.GetChildren(toggle).GetEnumerator();
            
            if(enumerator.MoveNext()){

                var tb = enumerator.Current as TextBlock;

                OnToggleClick(toggle.IsChecked, tb.Text);
            }
        }

        private void OnToggleClick(bool? isChecked, String name)
        {
            if (isChecked.Value)
            {
                AddChannel(name);
            }
            else
            {
                Candinate.deleteByName(name);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private string keystore_file_path;
        public string KeystoreFilePath
        {
            get { return keystore_file_path; }
            set
            {
                keystore_file_path = value;
                NotifyPropertyChanged("KeystoreFilePath");
            }
        }

        private string key_store_pw;
        public string KeyStorePw
        {
            get { return key_store_pw; }
            set
            {
                if (value != key_store_pw)
                {
                    key_store_pw = value;
                    NotifyPropertyChanged("KeyStorePw");
                }
            }
        }

        private string alias_pw;
        public string AliasPw
        {
            get { return alias_pw; }
            set
            {
                if (value != alias_pw)
                {
                    alias_pw = value;
                    NotifyPropertyChanged("AliasPw");
                }
            }
        }

        private string alias;
        public string Alias
        {
            get { return alias; }
            set
            {
                if (value != alias)
                {
                    alias = value;
                    NotifyPropertyChanged("Alias");
                }
            }
        }

        private string setting_file;
        public string SettingFile
        {
            get { return setting_file; }
            set
            {
                if (value != setting_file)
                {
                    setting_file = value;
                    NotifyPropertyChanged("SettingFile");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class EditItem : ChannelItem
    {
        public EditItem(String name, EditState state):base(name)
        {
            State = state;
        }

        private EditState state;
        public EditState State
        {
            get { return state; }
            set
            {
                if (value != state)
                {
                    state = value;
                    NotifyPropertyChanged("State");
                }
            }
        }
    }

    public class TemplateItem : ChannelItem
    {
        public TemplateItem(String name, bool isCheck)
            : base(name)
        {
            IsChecked = isCheck;
        }

        private bool isChecked;
        public bool IsChecked 
        {
            get { return isChecked; }
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;
                    NotifyPropertyChanged("IsChecked");
                }
            }
        }
    }

    public class ChannelItem : INotifyPropertyChanged
    {
        public ChannelItem(String name)
        {
            ItemName = name;
        }
        private String itemName;
        public String ItemName 
        {
            get { return itemName; }
            set
            {
                if (value != itemName)
                {
                    itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

        }
    }
}
