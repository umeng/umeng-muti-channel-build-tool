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
        private ObservableCollection<PackageState> AvailabelChannels = new ObservableCollection<PackageState>();
        private ApkInfo mApkInfo = new ApkInfo();


        BackgroundWorker bw = new BackgroundWorker();

        public ChannelTool()
        {
            InitializeComponent();

            LoadConfigrationAndBindList();

            //InitBackgroundWorker();

            //DataContext = mApkInfo;
        }

        private void LoadConfigrationAndBindList()
        {
            //if (CandinateConfigrationFiles.Count > 0)
            //{
            //    CandinateConfigrationFiles.Clear();
            //}

            //string[] files = ProjectConfigration.GetConfigFileList();
            //if (files != null)
            //{
            //    foreach (string file in files)
            //    {
            //        CandinateConfigrationFiles.Add(file);
            //    }
            //}

            //this.settingList.ItemsSource = CandinateConfigrationFiles;

            //if (CandinateConfigrationFiles.Count > 0)
            //{
            //    var config = ProjectConfigration.readSettingFromFile(CandinateConfigrationFiles[0]);

            //    foreach (ChannelItem item in config.Candinate)
            //    {
            //        AvailabelChannels.Add(new PackageState( item.ChannelName, State.START ));
            //    }
            //}

            for (int i = 0; i < 10; i++)
            {
                AvailabelChannels.Add(new PackageState("appchina0", State.EMPTY));
                AvailabelChannels.Add(new PackageState("appchina1", State.EMPTY));
                AvailabelChannels.Add(new PackageState("appchina2", State.EMPTY));
                AvailabelChannels.Add(new PackageState("appchina3", State.EMPTY));
            }

            this.Channels.ItemsSource = AvailabelChannels;

            //this.hint.SetBinding(Label.ContentProperty, new Binding("mApkFile"));
            
        }
        /*
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
            PackageState state = e.UserState as PackageState;

            int index = e.ProgressPercentage;

            if (index < AvailabelChannels.Count)
            {
                AvailabelChannels[index].CurrentState = state.CurrentState;
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

            this.hint.Content = System.IO.Path.GetFileName( path );

            // About apk file
            mApkInfo.parseApk(path); 
        }

        /// <summary>
        /// Edit setting file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            //string configFile = this.settingList.SelectedValue as string;

            //var dialog = new ConfigTemplate();

            //dialog.SetConfigTemplateContext( configFile );
            //bool? result = dialog.ShowDialog();

            //if (result != null)
            //{
            //    LoadConfigrationAndBindList();
            //}
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

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
        }*/
    }
}
