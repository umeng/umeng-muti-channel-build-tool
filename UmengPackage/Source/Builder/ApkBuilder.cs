using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

using UmengPackage.Source.Common;
using UmengPackage.Source.Model;
using CommonTools;

namespace UmengPackage.Source
{
    /// <summary>
    /// dir/temp/decode-folder
    /// dir/unsigned-{application}.apk
    /// dir/unziplaligned-{application}.apk
    /// dir/{application}-channel.apk
    /// dir/tools/apktool/.bat
    /// </summary>
    class ApkBuilder : Builder
    {
        string PathToApktool { get; set; }
        
        DecodedApkStruct ApkFolderStruct { get; set; }
        
        public ApkBuilder(ProjectConfigration config,DecodedApkStruct das, BackgroundWorker monitor) : base(config, das.AppName, monitor)
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


        public override void BuildUnsignedApk()
        {
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
        public override string GetAndroidManifestPath()
        {
            return ApkFolderStruct.AxmlFile;
        }

        public override string GetUnsignedApk()
        {
            return Path.Combine(CurrentDir, string.Format( "unsigned-{0}.apk", ApplicationName ));
        }

        public override string GetUnzipAlignedApk()
        {
            return Path.Combine(CurrentDir, string.Format("unzipAligned-{0}.apk", ApplicationName));
        }

        public override string GetFinalApk(string channel)
        {
            return Path.Combine(CurrentDir, string.Format("{0}-{1}.apk", ApplicationName, channel));
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
    }
}
