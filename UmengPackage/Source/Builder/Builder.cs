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
        private readonly string pathToJarsigner = Path.Combine(Environment.CurrentDirectory, "tools", "SignApk.jar");
        private readonly string pathToZipalign = Path.Combine(Environment.CurrentDirectory, "tools", "zipalign.exe");
        //BackgroudWorker monitor ,used to publish progress
        protected BackgroundWorker monitor;
        public ProjectConfigration Config { get; set; }
        public String CurrentDir { get; set; }
        public String ApplicationName { get; set; }
        
        public Builder() 
        {
            CurrentDir = System.Environment.CurrentDirectory;
        }
        public Builder(ProjectConfigration config, String applicationName, BackgroundWorker bw) 
        {
            Config = config;
            ApplicationName = applicationName;
            monitor = bw;

            CurrentDir = System.Environment.CurrentDirectory;
        }

        public abstract void Backup();

        public void Build()
        {
            SetProjectEnvironmet();
            int count = Config.Candinate.Count;
            int i = 0;
            int per = 0;
            //PackageState state = new PackageState();

            //foreach (EditItem channel in Config.Candinate )
            //{
            //    i++;
            //    per = i*100/count;
            //    state.setChannel(channel.ChannelName);
            //    //start
            //    monitor.ReportProgress(per, state.setState(State.START));
            //    try
            //    {
            //        ReplaceChannle(channel.ChannelName);

            //        BuildUnsignedApk();

            //        SignAPK(channel.ChannelName);
            //        ZipAlign(channel.ChannelName);

            //        CopyToWorkspace(channel.ChannelName);

            //        monitor.ReportProgress(per, state.setState( State.END));
            //    }
            //    catch (Exception e)
            //    {
            //        monitor.ReportProgress(per, state.setState( State.FAILURE));
            //        throw e;
            //    }
                //end
            //}
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
                channel_mata.SetAttribute("android:name", "UMENG_CHANNEL");
                channel_mata.SetAttribute("android:value", channel);

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
                throw new Exception(string.Format("Can't find unsigned apk at {0}", unSignedApk));
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
                throw new Exception(string.Format("Can't find unzipAligned apk at {0}", unzipAlignedApk));
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
                throw new Exception("Fail to generate .apk for " + channel);
            }

            string dst_file = generateDstFile(channel);
            if (File.Exists(dst_file)) File.Delete(dst_file);

            File.Copy(apk_file, dst_file);
        }

        private string generateDstFile(string channel)
        {

            string project_name = "";
            string file_name = string.Format("{0}_{1}.apk", project_name, channel);

            string dst_path = Path.Combine(System.Environment.CurrentDirectory,
                Path.Combine("output", project_name));

            if (!Directory.Exists(dst_path))
            {
                Directory.CreateDirectory(dst_path);
            }

            return Path.Combine(dst_path, file_name);
        }
    }
}
