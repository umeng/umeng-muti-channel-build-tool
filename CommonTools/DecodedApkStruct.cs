using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace CommonTools
{
    public class DecodedApkStruct
    {
        public string AxmlFile = null;
        public string ApktoolYML = null;
        public string ResFolder = null;
        public string Asserts = null;
        public string Smali = null;
        public string CommonFolder = null;

        public string AppName = null;
        public string VersionName = null;
        public string VersionCode = null;
        public string IconPath = null;

        public string Channel = null;
        public string Appkey = null;

        private static Regex[] r = { 
                            new Regex("versionName=\"(.*?)\""),
                            new Regex("versionCode=\"(.*?)\""),
                            new Regex("icon=\"(.*?)\""),
                            new Regex("label=\"(.*?)\""),
                          };
        private static string strReg = "\"{0}\">(.*?)<";


        public DecodedApkStruct(string root)
        {
            Root = root;

            AxmlFile = Path.Combine( root, "AndroidManifest.xml");
            ApktoolYML = Path.Combine(root, "apktool.yml");
            ResFolder = Path.Combine(root, "res");
            Asserts = Path.Combine(root, "assets");
            Smali = Path.Combine(root, "smali");
            CommonFolder = Path.Combine(root, "smali", "com", "umeng", "common");
        }

        public string Root { get; set; }
        /// <summary>
        /// if sdk is confused , there must be at least one a.smali or b.smali file
        /// </summary>
        public bool IsConfused 
        {
            get
            {
                foreach (string filename in Directory.EnumerateFiles(CommonFolder))
                {
                    if ("a.smali".Equals(filename) || "b.smali".Equals(filename))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        //log file is a.smali or Log.smali
        public string LogFile {
            get
            {
                string logFile = "Log.smali";

                if (IsConfused)
                {
                    logFile = "a.smali";
                }

                return Path.Combine(CommonFolder, logFile );
            }
        }

        public DecodedApkStruct parseAxml()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AxmlFile);


                var attr_version = doc.DocumentElement.Attributes;

                
                VersionName = fetchString(attr_version["android:versionName"].Value);
                VersionCode = fetchString(attr_version["android:versionCode"].Value);

                var application = doc.GetElementsByTagName("application")[0];
                var attr_app = application.Attributes;

                AppName = fetchString(attr_app["android:label"].Value);

                IconPath = searchLogo(attr_app["android:icon"].Value.Substring("@drawable/".Length) + ".png");


                //update 
                var mata = doc.GetElementsByTagName("meta-data");

                foreach (XmlElement mata_data in mata)
                {
                    if (mata_data.GetAttribute("android:name").Equals("UMENG_CHANNEL"))
                    {
                        Channel = mata_data.GetAttribute("android:value");
                        continue;
                    }

                    if (mata_data.GetAttribute("android:name").Equals("UMENG_APPKEY"))
                    {
                        Appkey = mata_data.GetAttribute("android:value");
                        continue;
                    }
                }

                return this;
            }
            catch(Exception e) {
                throw new Exception("Parsing 'AndroidManifest.xml' error : " + e.Message);
            }

            return null;
        }
        //<string name="app_name">UmengDemo</string>
        private string fetchString(string origin)
        {
            if (origin.StartsWith("@"))
            {
                string id = origin.Substring("@string/".Length);
                string stringData = File.ReadAllText(Path.Combine(ResFolder, "values", "strings.xml"));
                Regex reg = new Regex(string.Format(strReg, id));
                return reg.Match(stringData).Groups[1].Value;
            }

            return origin;
        }

        /// <summary>
        /// search res/drawable-xxxx/xxx.png
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string searchLogo(String filename)
        {
            var folder = Directory.GetDirectories(ResFolder);

            foreach( string path in Directory.GetDirectories(ResFolder).Where( T=> Path.GetFileName(T).StartsWith("drawable")))
            {
                foreach( string subPath in Directory.GetFiles( path))
                {
                    if ( Path.GetFileName(subPath).Equals(filename))
                    {
                        return subPath;
                    }
                }
            }

            return null;
        }
    }
}
