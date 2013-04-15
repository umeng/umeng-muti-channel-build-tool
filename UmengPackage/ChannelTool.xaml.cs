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

namespace UmengPackage
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ChannelTool : UserControl
    {
        private string mApkFile = "Drag APK Here";
        private string mConfigFile = null;

        private ObservableCollection<string> CandinateConfigrationFiles = new ObservableCollection<string>();
        private ObservableCollection<ShowItem> AvailabelChannels = new ObservableCollection<ShowItem>();
        
        private ApkInfo mApkInfo = new ApkInfo();


        BackgroundWorker bw = new BackgroundWorker();

        public ChannelTool()
        {
            InitializeComponent();

            LoadConfigrationAndBindList();

            DataContext = mApkInfo;
        }

        private void LoadConfigrationAndBindList()
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

            if (CandinateConfigrationFiles.Count > 0)
            {
                var config = ProjectConfigration.readSettingFromFile(CandinateConfigrationFiles[0]);
                
                AvailabelChannels.Clear();

                foreach (string item in config.Candinate)
                {
                    AvailabelChannels.Add(new ShowItem(item,0));
                }
            }

            this.Channels.ItemsSource = AvailabelChannels;
        }

        private void OnEdit(object sender, RoutedEventArgs e)
        {

            var filename = this.settingList.SelectedValue as string;

            var dialog = new ConfigTemplate( filename );

            //dialog.TranslatePoint(new Point(-200, -100), this);

            bool? result = dialog.ShowDialog();

            if (result != null)
            {
                LoadConfigrationAndBindList();
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
                w.setProject(mApkFile);
                w.setProfile(GetProfile());

                w.start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProjectConfigration GetProfile()
        {
            return ProjectConfigration.readSettingFromFile( mConfigFile );
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Progress changed

            int index = (int)(e.UserState);

            int step = e.ProgressPercentage;

            if (index < AvailabelChannels.Count)
            {
                AvailabelChannels[index].Progress = step;
            }
           
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Backgroud work complete !

            if ((e.Cancelled == true))
            {
                MessageBox.Show("任务已经取消");
            }

            else if (!(e.Error == null))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(e.Error.Message);
                sb.Append("\n\n");
                sb.Append("查看 /log/i.txt 详细错误信息");
                MessageBox.Show(sb.ToString());
            }

            else
            {
                MessageBox.Show("渠道打包完成");
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
            System.Diagnostics.Debug.WriteLine( path );
            
            this.ProcessHint.Visibility = Visibility.Visible;
            // About apk file
            mApkInfo.parseApkAsync(path,
            (T) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ////end
                    if (T != null)
                    {
                        mApkInfo.bind(T);
                        ProcessHint.Visibility = Visibility.Hidden;
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

                bw.RunWorkerAsync();
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
            return true;
        }

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
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        public ShowItem(string name, int progress):base( name)
        {
            this.progress = progress;
        }
    }
}
