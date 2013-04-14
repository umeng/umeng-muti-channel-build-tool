using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.Packaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Windows;

using UmengPackage.Source.Common;
using CommonTools;
using System.Threading;
using System.Windows.Threading;



namespace UmengPackage.Source.Model
{
    class ApkInfo : INotifyPropertyChanged
    {
        public delegate void OnParseEnd(DecodedApkStruct das);

        public OnParseEnd end;

        /// <summary>
        /// Same as ApkBuild's temp folder
        /// </summary>
        private static string mTempFolder = Path.Combine(Environment.CurrentDirectory, "temp");
        private string apkPath = null;



        public void parseApkAsync(string path, OnParseEnd end)
        {
            this.apkPath = path;
            this.end = end;

            new Thread(parseApk).Start();
        }

        public void parseApk()
        {
            Aapt.DecodeApk(apkPath, mTempFolder);

            var dfs = new DecodedApkStruct(mTempFolder).parseAxml();
           
            end(dfs);
        }

        public void bind(DecodedApkStruct das)
        {
            var dfs = das;

            if (dfs == null)
            {
                AppName = Path.GetFileName(apkPath);
                AppVersionName = "";
                AppVersionCode = "";

                ApkHolderState = Visibility.Visible;
            }
            else
            {

                AppName = dfs.AppName;
                AppVersionName = dfs.VersionName;
                AppVersionCode = dfs.VersionCode;
                AppSize = formatFileSize(apkPath);

                if (File.Exists(dfs.IconPath))
                {
                    MemoryStream memoryStream = new MemoryStream();

                    byte[] fileBytes = File.ReadAllBytes(dfs.IconPath);
                    memoryStream.Write(fileBytes, 0, fileBytes.Length);
                    memoryStream.Position = 0;

                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = memoryStream;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();

                    AppIcon = bi;

                    ApkHolderState = Visibility.Hidden;
                }
            } 
        }

        private Visibility apkHolderState;
        public Visibility ApkHolderState
        {
            get 
            {
                return apkHolderState; 
            }
            set
            {
                apkHolderState = value;
                NotifyPropertyChanged("ApkHolderState");
            }
        }
        
        private BitmapImage appIcon;
        public BitmapImage AppIcon
        {
            get { return appIcon; }
            set
            {
                if (appIcon != value)
                {
                    appIcon = value;
                    NotifyPropertyChanged("AppIcon");
                }
            }
        }
        private string appName;
        public string AppName
        {
            get { return appName; }
            set
            {
                if (appName != value)
                {
                    appName = value;
                    NotifyPropertyChanged("AppName");
                }
            }
        }

        private string appVersionCode;
        public string AppVersionCode
        {
            get { return appName; }
            set
            {
                if (appName != value)
                {
                    appName = value;
                    NotifyPropertyChanged("AppVersionCode");
                }
            }
        }

        private string appVersion;
        public string AppVersionName
        {
            get { return appVersion; }
            set
            {
                if (appVersion != value)
                {
                    appVersion = value;
                    NotifyPropertyChanged("AppVersion");
                }
            }
        }
        private string appSize;
        public string AppSize
        {
            get { return appSize; }
            set
            {
                if (appSize != value)
                {
                    appSize = value;
                    NotifyPropertyChanged("AppSize");
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

        private string formatFileSize(string filename)
        {
            string[] sizes = { "B", "K", "M", "G" };
            double len = new FileInfo(filename).Length;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
