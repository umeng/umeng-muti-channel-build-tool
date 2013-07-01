using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace CommonTools
{
    public class Aapt
    {
        private static string mPathToAapt = Path.Combine( "tools", "apktool", "aapt.exe");
        private static string mPathToApktool = Path.Combine( "tools", "apktool", "apktool.bat");
        private static string mPathToSigner = Path.Combine( "tools", "SignApk.jar");
        private static string mPathToZipAlign = Path.Combine( "tools", "zipalign.exe");
        private static string mPathToKeyTool = Path.Combine("tools","KeyTool.jar");

        private static string mPathToErrorLog = Path.Combine("log", "e.txt");

        /// <summary>
        /// Add apktool to env path, since apktool need aapt in path
        /// </summary>
        static Aapt()
        {
            var oldPath = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            var newPath = oldPath + ";" + Path.Combine("tools", "apktool");
            System.Environment.SetEnvironmentVariable("PATH", newPath );
        }
        
        /// <summary>
        /// 0 - cmd excute success
        /// 1 - keystore password is not right
        /// 2 - alias does not exist
        /// 3 - alias's passwor is not right
        /// </summary>
        /// <param name="path"></param>
        /// <param name="store_pw"></param>
        /// <param name="alias"></param>
        /// <param name="key_pw"></param>
        /// <returns></returns>
        public static int checkStoreAndAlias(string path, string store_pw, string alias, string key_pw)
        {
            List<string> cmd = new List<string>();

            cmd.Add("java");
            cmd.Add("-jar");
            cmd.Add(mPathToKeyTool);

            cmd.Add(string.Format("\"{0}\"", path));
            cmd.Add(store_pw);
            cmd.Add(alias);
            cmd.Add(key_pw);

            Sys.Run(cmd.ToCommand());

            int i = 0;
            try
            {
                string str = File.ReadAllText(mPathToErrorLog);
                System.Diagnostics.Debug.WriteLine("output is : " + str);
                i = int.Parse( str );
            }
            catch { }

            return i;
        }

        /// <summary>
        /// return 
        /// { 
        /// version name,
        /// version code,
        /// icon
        /// label
        /// }
        /// </summary>
        /// <param name="pathToApk"></param>
        /// <returns></returns>
        public static string getAppInfo(string pathToApk)
        {
            List<string> cmd = new List<string>();
            cmd.Add(mPathToAapt);
            cmd.Add("d");
            cmd.Add("badging");
            cmd.Add( pathToApk );

            StringBuilder data = new StringBuilder();

            Sys.Run(cmd.ToCommand(), (S,E) =>
            {
                data.Append(E.Data);
               
            });
            return data.ToString();
        }

        public static void DecodeApk(string pathToApkFile, string pathToDecodeFolder)
        {
            if (!File.Exists(pathToApkFile))
            {
                throw new Exception("Target apk is missing..");
            }

            List<String> cmd = new List<string>();
            cmd.Add(mPathToApktool);
            cmd.Add("d");
            cmd.Add("--no-src");
            cmd.Add("-f");
            cmd.Add(string.Format("\"{0}\"", pathToApkFile));
            cmd.Add(string.Format("\"{0}\"", pathToDecodeFolder));

            Sys.Run(cmd.ToCommand());
        }

        public static void DecodeApkWithSource(string pathToApkFile, string pathToDecodeFolder)
        {
            if (!File.Exists(pathToApkFile))
            {
                throw new Exception("Target apk is missing..");
            }

            List<String> cmd = new List<string>();
            cmd.Add(mPathToApktool);
            cmd.Add("d");
            cmd.Add("-f");
            cmd.Add(string.Format("\"{0}\"", pathToApkFile));
            cmd.Add(string.Format("\"{0}\"", pathToDecodeFolder));

            Sys.Run(cmd.ToCommand());
        }


        public static void BuildApk(string pathToDecodeFolder, string pathToDstApk)
        {
            if (File.Exists(pathToDstApk))
            {
                File.Delete(pathToDstApk);
            }

            List<String> cmd = new List<string>();
            cmd.Add(mPathToApktool);
            cmd.Add("b");
            cmd.Add(string.Format("\"{0}\"", pathToDecodeFolder));
            cmd.Add(string.Format("\"{0}\"", pathToDstApk));

            Sys.Run(cmd.ToCommand());
        }


        /// <summary>
        /// cmd: ApkSigner keystore storepw alias password input output
        /// </summary>
        /// <param name="channel"></param>
        public static void SignAPK(string keystore, string storepw, string entry, string keypw, string source, string dst)
        {
            string unSignedApk = source;
            string unzipAlignedApk = dst;

            if (!File.Exists(unSignedApk))
            {
                throw new Exception(string.Format("Can't find unsigned apk at {0}", unSignedApk));
            }

            if (File.Exists(unzipAlignedApk))
            {
                File.Delete(unzipAlignedApk);
            }

            List<string> cmd = new List<string>();

            cmd.Add("java");
            cmd.Add("-jar");
            cmd.Add( mPathToSigner );

            cmd.Add(string.Format("\"{0}\"",keystore));
            cmd.Add(storepw);
            cmd.Add(entry);
            cmd.Add(keypw);

            cmd.Add(string.Format("\"{0}\"",unSignedApk));
            cmd.Add(string.Format("\"{0}\"",unzipAlignedApk));

            Sys.Run(cmd.ToCommand());
        }

        public static void ZipAlign(string source ,string dst)
        {
            string unzipAlignedApk = source;
            string finalApk = dst;

            if (!File.Exists(unzipAlignedApk))
            {
                throw new Exception(string.Format("Can't find unzipAligned apk at {0}", unzipAlignedApk));
            }
            if (File.Exists(finalApk))
            {
                File.Delete(finalApk);
            }
            List<string> cmd = new List<string>();

            cmd.Add( mPathToZipAlign );
            cmd.Add("-v");
            cmd.Add("4");
            cmd.Add(string.Format("\"{0}\"",unzipAlignedApk)); //input apk
            cmd.Add(string.Format("\"{0}\"",finalApk)); //output apk

            Sys.Run(cmd.ToCommand());
        }
    }
}
