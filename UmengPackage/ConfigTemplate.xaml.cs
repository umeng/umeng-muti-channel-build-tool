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
using UIControls;

using UmengPackage.Source.Model;

namespace UmengPackage
{
    /// <summary>
    /// Interaction logic for ChannelTemplate.xaml
    /// </summary>
    public partial class ConfigTemplate : Window
    {
        ProjectConfigration config = null;
        //ObservableCollection<EditItem> Template = new ObservableCollection<EditItem>();
        ObservableCollection<EditItem> Candinate = new ObservableCollection<EditItem>();

        public ConfigTemplate()
        {
            InitializeComponent();

            for (int i = 0; i < 2; i++)
            {
                Candinate.Add(new EditItem("GooglePlay", EditState.Normal));
            }

            Candinate.Add(new EditItem("",EditState.Editable));

            this.Channels.ItemsSource = Candinate;
        }

        public void SetConfigTemplateContext(string fileName)
        {
            //load configration
            //if (!string.IsNullOrEmpty(fileName))
            //{
            //    config = ProjectConfigration.readSettingFromFile(fileName);
            //    this.tb_setting_file.Text = fileName;
            //}
            //else
            //{
            //    config = new ProjectConfigration();
            //}
            //load template
            //LoadTemplate();
            //bind context
            //this.DataContext = config;
            //this.Channels.ItemsSource = Candinate;
            //this.StandardChannelTemplate.ItemsSource = Template;
        }

        public void LoadTemplate()
        {
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "GooglePlay"));
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "Anzhi"));
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "Anzhuo"));
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "Appchina"));
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "GooglePlay1"));
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "GooglePlay2"));
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "GooglePlay3"));
            //Template.Add(new EditItem("Images/icon.png", "Images/add.png", "GooglePlay4"));

            //var coincide = from A in Template
            //               from B in config.Candinate
            //               where A.ChannelName.Equals(B.ChannelName)
            //               select A;

            //foreach (EditItem item in coincide)
            //{
            //    item.EditorImage = "Images/ready.png";
            //}
        }

        /// <summary>
        /// Add Template channel to candinate channels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            // cast the sender to a button
            //Button button = e.OriginalSource as Button;

            //// find the item that is the datacontext for this button
            //EditItem channel = (EditItem)button.DataContext;

            //System.Diagnostics.Debug.WriteLine(string.Format("Template add {0} button is clicked !", channel.ChannelName));


            //foreach (EditItem channelItem in config.Candinate.Where(T => T.ChannelName.Equals(channel.ChannelName)))
            //{
            //    System.Diagnostics.Debug.WriteLine("已经添加过了");
            //    return;
            //}

            //channel.EditorImage = "Images/ready.png";

            //config.Candinate.Add(new EditItem()
            //{
            //    ChannelIcon = channel.ChannelIcon,
            //    EditorImage = "Images/remove.png",
            //    ChannelName = channel.ChannelName
            //});


        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Candinate channel remove button is clicked!");
            // cast the sender to a button
            //Button button = e.OriginalSource as Button;

            // find the item that is the datacontext for this button
            //EditItem channel = button.DataContext as EditItem;

            //System.Diagnostics.Debug.WriteLine(string.Format("Template remove {0} button is clicked !", channel.ChannelName));

            //foreach (EditItem channelItem in Template.Where(T => T.ChannelName.Equals(channel.ChannelName)))
            //{
            //    System.Diagnostics.Debug.WriteLine("Reset template state");

            //    channelItem.EditorImage = "Images/add.png";
            //    break;
            //}

            //config.Candinate.Remove(channel);
        }
        //add channel
        private void AddChannel_Click(object sender, RoutedEventArgs e)
        {
            //var dialog = new ChannelEditorWindow();
            //bool? result = dialog.ShowDialog();
            //string value = dialog.getChannels();

            //if (result != null && value != null)
            //{
            //    System.Diagnostics.Debug.WriteLine("value:" + result.Value + " result:" + result);
            //    string[] channels = value.Split(new char[] { ',',';',':'});

            //    foreach (string channel in channels)
            //    {
            //        //config.Candinate.Add(new EditItem("Images/icon.png", "Images/remove.png", channel));
            //    }
            //}
        }

        private void SearchTextBox_FindJava(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = null;
                if (isValidJavaSDKPath(path = folderBrowserDialog1.SelectedPath))
                {
                    config.Java_Path = path;
                }
                else
                {
                    MessageBox.Show("请选择 JDK 工程根目录( 包涵 lin ,bin 等子目录)");
                }
            }
        }

        private void SearchTextBox_FindAndroid(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = null;
                if (isValidAndroidSDKPath(path = folderBrowserDialog1.SelectedPath))
                {
                    config.Android_Path = path;
                }
                else
                {
                    System.Windows.MessageBox.Show("请选择 Android 工程根目录");
                }
            }
        }

        private void SearchTextBox_FindKeyStore(object sender, RoutedEventArgs e)
        {
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            openFileDialog1.DefaultExt = "keystore";
            openFileDialog1.Filter = "keystore files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                config.Keystore_File_Path = openFileDialog1.FileName;
            }
        }

        public static bool isValidAndroidSDKPath(string path)
        {
            string[] folders = { "tools", "platforms" };
            string folder_path;

            foreach (string folder in folders)
            {
                folder_path = System.IO.Path.Combine(path, folder);

                if (!System.IO.Directory.Exists(folder_path))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool isValidJavaSDKPath(string path)
        {
            string bin = System.IO.Path.Combine(path, "bin");
            string lib = System.IO.Path.Combine(path, "lib");

            if (System.IO.Directory.Exists(bin) && System.IO.Directory.Exists(lib))
            {
                return true;
            }

            return false;
        }

        //Save current configration
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckInput();

                config.writeSettintToFile( this.tb_setting_file.Text );

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
            if (string.IsNullOrEmpty(config.Keystore_File_Path))
            {
                throw new Exception("没有设置 keystore");
            }

            //keystore pw

            if (string.IsNullOrEmpty(config.Keystore_Pw))
            {
                throw new Exception("没有设置 keystore password");
            }

            //alias

            if (string.IsNullOrEmpty(config.Alias))
            {
                throw new Exception("没有设置 keystore entry (alias)");
            }

            //entry pw

            if (string.IsNullOrEmpty(config.Key_Pw))
            {
                throw new Exception("没有设置 keystore entry password");
            }

            //android sdk path
            if (string.IsNullOrEmpty(config.Android_Path))
            {
                throw new Exception("没有设置 Android SDK Path");
            }

            //java home

            if (string.IsNullOrEmpty(config.Java_Path))
            {
                throw new Exception("没有设置 Java 路径");
            }
            //channel
            if (config.Candinate == null || config.Candinate.Count == 0)
            {
                throw new Exception("没有设置 渠道 ");
            }

            //setting file name

            if (string.IsNullOrEmpty(this.tb_setting_file.Text))
            {
                throw new Exception("亲，起个名呗 ^_^ ");
            }

            return true;
        }


    }

    public class EditItem
    {
        public EditItem(String name, EditState state)
        {
            ItemName = name;
            State = state;
        }

        public String ItemName
        {
            get;
            set;
        }
        public EditState State
        {
            get;
            set;
        }
    }
}
