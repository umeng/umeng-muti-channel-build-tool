using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

using UmengPackage.Source.Common;
using UmengPackage.Source.Model;
using CommonTools;
using System.Xml;

namespace UmengPackage.Source
{
    /// <summary>
    /// dir/temp/decode-folder
    /// dir/apk_temp/unsigned-{application}.apk
    /// dir/apk_temp/unziplaligned-{application}.apk
    /// dir/apk_temp/{application}-channel.apk
    /// dir/tools/apktool/.bat
    /// </summary>
    class ApkBuilder : Builder
    {
        string PathToApktool { get; set; }
        
        DecodedApkStruct ApkFolderStruct { get; set; }

        public ApkBuilder(ProjectConfigration config, DecodedApkStruct das, BackgroundWorker monitor, DoWorkEventArgs e)
            : base(config, das.AppName, monitor,e)
        {
            ApkFolderStruct = das;
        }
        
        public override void SetProjectEnvironmet()
        {
            //Add apktool to environment
            PathToApktool = Path.Combine(CurrentDir, "tools", "apktool", "apktool.bat"); 
            //Encode YML file<chinese char will result in build 'malcharset' exception >
            EncodeYML();
        }

        private void EncodeYML()
        {
            if(!File.Exists( ApkFolderStruct.ApktoolYML))
            {
                return;
            }
            try
            {
                string[] lines = File.ReadAllLines(ApkFolderStruct.ApktoolYML);

                for (int i = 0; i < lines.Length; i++)
                {
                    if( lines[i].Contains("apkFileName"))
                    {
                        lines[i] = string.Format("apkFileName: temp_{0}.apk", DateTime.Now.Second);
                        break;
                    }
                }

                File.WriteAllLines(ApkFolderStruct.ApktoolYML, lines);

            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Fail to encode apktool.yml file");
            }
        }


        public override void BuildUnsignedApk(string channel)
        {
            ReplaceChannle(channel);
            string tempFolder = GetTempFolder();

            if (!Directory.Exists( tempFolder ))
            {
                throw new Exception(string.Format("Can't find decoded folder at {0}", tempFolder ));
            }

            Aapt.BuildApk(tempFolder, GetUnsignedApk());
        }

        public string GetTempFolder()
        {
            return ApkFolderStruct.Root;
        }

        public override void Backup()
        {
        }

        public override void Restore()
        {
            string temp = GetTempFolder();

            if( Directory.Exists( temp ))
            {
                Directory.Delete( temp );
            }

            string[] files = Directory.GetFiles(CurrentDir);

            foreach( string file in files.Where( T => T.EndsWith(".apk")))
            {
                File.Delete(file);
            }
        }

        public void ReplaceChannle(string channel)
        {

            string androidmanifest_file = ApkFolderStruct.AxmlFile;

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
    }
}
