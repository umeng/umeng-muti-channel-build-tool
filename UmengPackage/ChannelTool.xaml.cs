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
using System.ComponentModel;

using UmengPackage.Source.Model;
using UmengPackage.Source.Common;
using UmengPackage.Source;
using System.Collections.ObjectModel;
using System.Threading;

using CommonTools;

namespace UmengPackage
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ChannelTool : UserControl
    {
        private string mConfigFile = null;

        private int mSelectedConfigIndex = 0;

        private ObservableCollection<string> CandinateConfigrationFiles = new ObservableCollection<string>();
        private ObservableCollection<ShowItem> AvailabelChannels = new ObservableCollection<ShowItem>();
        
        private ApkInfo mApkInfo = new ApkInfo();


        BackgroundWorker bw = new BackgroundWorker();

        public ChannelTool()
        {
            InitializeComponent();
            InitBackgroundWorker();
            LoadConfigration();

            DataContext = mApkInfo;
            settingList.SelectionChanged += new SelectionChangedEventHandler(settingList_SelectionChanged);

            var setting = Preferences.getDefault();

            mSelectedConfigIndex = setting.getInt("selected_index") ?? 0;

            BindList();

            Application.Current.MainWindow.Closed += new EventHandler(MainWindow_Closed);
        }

        private void ResetChannelState()
        {
            foreach (ShowItem item in AvailabelChannels)
            {
                item.Progress = 0;
            }
        }

        void settingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mSelectedConfigIndex = settingList.SelectedIndex;

            BindList();
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            var editor = Preferences.getDefault().Editor();

            editor.WriteInt("selected_index", mSelectedConfigIndex).Commit();
        }

        private void LoadConfigration()
        {
            if (CandinateConfigrationFiles.Count > 0)
            {
                CandinateConfigrationFiles.Clear();
            }

            string[] files = ProjectConfigration.GetConfigFileList();
            if (files != null)
            {
                foreach (string file in files)
                {
                    CandinateConfigrationFiles.Add(file);
                }
            }

            this.settingList.ItemsSource = CandinateConfigrationFiles;
        }

        private void BindList()
        {
            if (mSelectedConfigIndex < 0 || mSelectedConfigIndex >= CandinateConfigrationFiles.Count)
            {
                mSelectedConfigIndex = 0;
            }

            this.settingList.SelectedIndex = mSelectedConfigIndex;

            if (CandinateConfigrationFiles.Count > 0)
            {
                var config = ProjectConfigration.readSettingFromFile(CandinateConfigrationFiles[mSelectedConfigIndex]);

                AvailabelChannels.Clear();

                foreach (string item in config.Candinate)
                {
                    AvailabelChannels.Add(new ShowItem(item, 0));
                }
            }

            this.Channels.ItemsSource = AvailabelChannels;
        }

        private void OnEdit(object sender, RoutedEventArgs e)
        {

            var filename = this.settingList.SelectedValue as string;

            var dialog = new ConfigTemplate( filename );

            bool? result = dialog.ShowDialog();

            if (result != null)
            {
                LoadConfigration();
                BindList();
            }
        }
       
        private void InitBackgroundWorker()
        {
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(doWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void doWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;

                Worker w = Worker.Instanse();
                
                w.setMoniter(worker);
                w.setProject(mApkInfo.DeApkStruct);
                w.setConfigure(GetConfigure());

                w.start();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public ProjectConfigration GetConfigure()
        {
            return ProjectConfigration.readSettingFromFile( mConfigFile );
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Progress changed
            System.Diagnostics.Debug.WriteLine("Progress changed :" + e.ProgressPercentage);

            int index = (int)(e.UserState);

            int step = e.ProgressPercentage;

            if (index >= 0 && index < AvailabelChannels.Count)
            {
                Channels.SelectedIndex = index;
                AvailabelChannels[index].Progress = step;
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Backgroud work complete !
            PackageHint.Visibility = Visibility.Hidden;

            if ((e.Cancelled == true))
            {
                MessageBox.Show("任务已经取消");
            }

            else if (!(e.Error == null))
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(e.Error.Message);
                sb.Append("\n\r");
                sb.Append(e.Error.StackTrace);
                sb.Append("\n\n");

                string logFile = System.IO.Path.Combine("log","e.txt");
                if( System.IO.File.Exists( logFile ))
                {
                    sb.Append( System.IO.File.ReadAllText( logFile));
                }

                MessageBox.Show(sb.ToString());
            }

            else
            {
                string path = System.IO.Path.Combine("output", mApkInfo.AppName);

                if (MessageBox.Show(String.Format("打开目录：\n {0} ", path), "渠道打包完成", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                {
                    //do nothing
                }
                else
                {
                    System.Diagnostics.Process.Start("explorer.exe", path);
                }
            }
        }

        private void dragDrop_Event(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length == 1 && files[0].isApkFile())
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;

                    if (e.RoutedEvent == DragDrop.DropEvent)
                    {
                        processFile(files[0]);
                    }

                    return;
                }
            }

            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void processFile(string path)
        {
            ResetChannelState();
            
            this.ParseHint.Visibility = Visibility.Visible;
            // About apk file
            mApkInfo.parseApkAsync(path,
            (T) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ////end
                    if (T)
                    {
                        mApkInfo.bind();
                        ParseHint.Visibility = Visibility.Hidden;
                    }
                }));
            }); 
        }

        private void btn_builder_Click(object sender, RoutedEventArgs e)
        {
            //loadConfig();
            try
            {
                if (!isEnviromentReady())
                {
                    return;
                }

                if (bw.IsBusy)
                {
                    MessageBox.Show("正在打包，稍后再试");
                    return;
                }

                ResetChannelState();

                bw.RunWorkerAsync();

                PackageHint.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool isEnviromentReady()
        {
            string configFile = this.settingList.SelectedValue as string;

            if (configFile == null)
            {
                throw new Exception("没有设置配置文件");
            }

            mConfigFile = configFile;

            if (mApkInfo.isApkReady())
            {
                throw new Exception("没有提供Apk文件");
            }

            return true;
        }


        public CancelEventHandler MainWindow_Closing { get; set; }
    }

    public class ShowItem : ChannelItem
    {
        private int progress;
        public int Progress 
        {
            get { return progress; }
            set
            {
                if (value != progress)
                {
                    progress = value;
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        public ShowItem(string name, int progress):base( name)
        {
            Progress = progress;
        }
    }
}
