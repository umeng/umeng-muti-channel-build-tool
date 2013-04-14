using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace CommonTools
{
    public class DecodedApkStruct
    {
        public string AxmlFile = null;
        public string ResFolder = null;
        public string Asserts = null;
        public string Smali = null;
        public string CommonFolder = null;

        public string AppName = null;
        public string VersionName = null;
        public string VersionCode = null;
        public string IconPath = null;

        private static Regex[] r = { 
                            new Regex("versionName=\"(.*?)\""),
                            new Regex("versionCode=\"(.*?)\""),
                            new Regex("icon=\"(.*?)\""),
                            new Regex("label=\"(.*?)\""),
                          };
        private static string strReg = "\"{0}\">(.*?)<";


        public DecodedApkStruct(string root)
        {
            AxmlFile = Path.Combine( root, "AndroidManifest.xml");
            ResFolder = Path.Combine(root, "res");
            Asserts = Path.Combine(root, "assets");
            Smali = Path.Combine(root, "smali");
            CommonFolder = Path.Combine(root, "smali", "com", "umeng", "common");
        }
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
                string appInfo = File.ReadAllText(AxmlFile, Encoding.UTF8);

                string[] result = new string[r.Length];

                for (int i = 0; i < r.Length; i++)
                {
                    var g = r[i].Match(appInfo).Groups;
                    result[i] = g[1].Value;
                }

                AppName = fetchString(result[3]);
                VersionName = fetchString(result[0]);
                VersionCode = fetchString(result[1]);

                IconPath = searchLogo(result[2].Substring("@drawable/".Length) + ".png");

                return this;
            }
            catch(Exception e) { }

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
