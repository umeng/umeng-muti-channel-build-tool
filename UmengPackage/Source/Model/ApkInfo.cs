using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.Packaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

using UmengPackage.Source.Common;
using CommonTools;



namespace UmengPackage.Source.Model
{
    class ApkInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// Same as ApkBuild's temp folder
        /// </summary>
        private static string mTempFolder = Path.Combine(Environment.CurrentDirectory, "temp");

        private static string IconPath = Path.Combine( Environment.CurrentDirectory, "icon.png" );

        public  void parseApk(string path)
        {
            Aapt.DecodeApk(path, mTempFolder);

            var dfs = new DecodedApkStruct(mTempFolder);

            AppName = dfs.AppName;
            AppVersionName = dfs.VersionName;
            AppVersionCode = dfs.VersionCode;
            AppSize = dfs.AppSize;

            string pathToIcon = dfs.IconPath;

            if (File.Exists(IconPath))
            {
                File.Delete(IconPath);
            }

            File.Copy(pathToIcon, IconPath);

            BitmapImage tempImage = new BitmapImage();
            tempImage.BeginInit();
            tempImage.StreamSource = File.Open(IconPath, FileMode.Open);
            tempImage.EndInit();

            AppIcon = tempImage;
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

        public static void extractAppIcon(string pathToApk, string pathInArchive, string des)
        {
            using (Package package = ZipPackage.Open(pathToApk, FileMode.Open, FileAccess.Read))
            {
                var part = package.GetPart(new Uri(pathInArchive));

                using (Stream source = part.GetStream(FileMode.Open, FileAccess.Read))
                {
                    FileStream targetFile = File.OpenWrite(des);
                    source.CopyTo(targetFile);
                    targetFile.Close();
                }
            }
        }
    }
}
