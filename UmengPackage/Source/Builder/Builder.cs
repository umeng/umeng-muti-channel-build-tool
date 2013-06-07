using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using UmengPackage.Source.Common;
using UmengPackage.Source.Model;
using System.ComponentModel;
using CommonTools;

namespace UmengPackage.Source
{
    abstract class Builder
    {
        private readonly string pathToJarsigner = Path.Combine("tools", "SignApk.jar");
        private readonly string pathToZipalign = Path.Combine("tools", "zipalign.exe");
        //BackgroudWorker monitor ,used to publish progress
        protected BackgroundWorker monitor;
        public ProjectConfigration Config { get; set; }
        public String CurrentDir { get; set; }
        public String ApplicationName { get; set; }
        
        public Builder() 
        {
            CurrentDir = "apk_temp";
            if (!Directory.Exists(CurrentDir))
            {
                Directory.CreateDirectory(CurrentDir);
            }
        }
        public Builder(ProjectConfigration config, String applicationName, BackgroundWorker bw):this()
        {
            Config = config;
            ApplicationName = applicationName;
            monitor = bw;
        }

        public abstract void Backup();

        public void Build()
        {
            SetProjectEnvironmet();
            int count = Config.Candinate.Count;
            int index = 0;
            int step = 0;
         
            foreach (string channel in Config.Candinate)
            {
                step = 0;

                //totally, 6 steps
                try
                {
                    monitor.ReportProgress(++step , index);
                    ReplaceChannle(channel);
                    
                    monitor.ReportProgress(++step, index);
                    BuildUnsignedApk();

                    monitor.ReportProgress(++step, index);
                    SignAPK(channel);

                    monitor.ReportProgress(++step, index);
                    ZipAlign(channel);

                    monitor.ReportProgress(++step, index);
                    CopyToWorkspace(channel);

                    monitor.ReportProgress(++step, index);

                    index++;
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("打包失败:{0}",e.Message),e);
                }
            }
        }
        public abstract void Restore();

        public abstract void BuildUnsignedApk();

        public abstract void SetProjectEnvironmet();

        public abstract string GetAndroidManifestPath();

        public abstract string GetUnsignedApk();
        public abstract string GetUnzipAlignedApk();
        public abstract string GetFinalApk(string channel);

        public void ReplaceChannle(string channel)
        {

            string androidmanifest_file = GetAndroidManifestPath();

            if (!File.Exists(androidmanifest_file))
            {
                throw new Exception(string.Format("Can't find AndroidManifest.xml file in the dir {0}", androidmanifest_file));
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(androidmanifest_file);

            //update 
            XmlNodeList mata_datas = doc.GetElementsByTagName("meta-data");
            bool hasSet = false;
            foreach (XmlElement mata_data in mata_datas)
            {
                if (mata_data.GetAttribute("android:name").Equals("UMENG_CHANNEL"))
                {
                    mata_data.SetAttribute("android:value", channel);
                    hasSet = true;
                    break;
                }
            }

            // if no set ,add it
            if (!hasSet)
            {
                XmlElement application = doc.GetElementsByTagName("application")[0] as XmlElement;

                XmlElement channel_mata = doc.CreateElement("meta-data");
                channel_mata.SetAttribute("name", "http://schemas.android.com/apk/res/android", "UMENG_CHANNEL");
                channel_mata.SetAttribute("value", "http://schemas.android.com/apk/res/android", channel);

                application.AppendChild(channel_mata);
            }

            doc.Save(androidmanifest_file);
        }

        /// <summary>
        /// cmd: ApkSigner keystore storepw alias password input output
        /// </summary>
        /// <param name="channel"></param>
        public void SignAPK(string channel)
        {
            string unSignedApk = GetUnsignedApk();
            string unzipAlignedApk = GetUnzipAlignedApk();

            if (!File.Exists(unSignedApk))
            {
                throw new Exception(string.Format("打包错误,没有生成中间文件:{1}", unSignedApk));
            }

            if (File.Exists(unzipAlignedApk))
            {
                File.Delete(unzipAlignedApk);
            }

            Aapt.SignAPK(Config.KeystoreFilePath, Config.KeystorePassword, Config.Alias, Config.AliasPassword,unSignedApk,unzipAlignedApk);
        }

        public void ZipAlign(string channel)
        {
            string unzipAlignedApk = GetUnzipAlignedApk();
            string finalApk = GetFinalApk(channel);

            if (!File.Exists(unzipAlignedApk))
            {
                throw new Exception(string.Format("签名错误，没有生成中间文件:{0}", unzipAlignedApk));
            }
            if (File.Exists(finalApk))
            {
                File.Delete(finalApk);
            }

            Aapt.ZipAlign(unzipAlignedApk, finalApk);
        }

        private void CopyToWorkspace(string channel)
        {
            string apk_file = GetFinalApk( channel );

            if (apk_file == null || !File.Exists(apk_file))
            {
                throw new Exception("ZipAlign 错误，没有生成优化包 ");
            }

            string dst_file = generateDstFile(channel);
            if (File.Exists(dst_file)) File.Delete(dst_file);

            File.Move(apk_file, dst_file);
        }

        private string generateDstFile(string channel)
        {

            string file_name = string.Format("{0}_{1}.apk", ApplicationName, channel);

            string dst_path = Path.Combine("output", ApplicationName);

            if (!Directory.Exists(dst_path))
            {
                Directory.CreateDirectory(dst_path);
            }

            return Path.Combine(dst_path, file_name);
        }
    }
}
