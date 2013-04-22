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
    public class ApkInfo : INotifyPropertyChanged
    {
        public delegate void OnParseEnd(bool isSuccess);

        public OnParseEnd end;

        /// <summary>
        /// Same as ApkBuild's temp folder
        /// </summary>
        private static string mTempFolder = Path.Combine(Environment.CurrentDirectory, "temp");
        private DecodedApkStruct mDeApkStruct = null;

        public DecodedApkStruct DeApkStruct
        {
            get { return mDeApkStruct; }
            set
            {
                if (mDeApkStruct != value)
                {
                    mDeApkStruct = value;
                }
            }
        }

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

            mDeApkStruct = new DecodedApkStruct(mTempFolder).parseAxml();

            end(mDeApkStruct != null? true:false);
        }

        public bool isApkReady()
        {
            return apkHolderState == Visibility.Visible;
        }

        public void bind()
        {
            var dfs = mDeApkStruct;

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
                AppSize = apkPath.formatFileSize();

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
    }
}
