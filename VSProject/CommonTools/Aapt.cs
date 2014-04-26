using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using Ionic.Zip;

namespace CommonTools
{
    public class Aapt
    {
        private static string mPathToAapt = Path.Combine( "tools", "apktool", "aapt.exe");
        private static string mPathToApktool = Path.Combine( "tools", "apktool", "apktool.bat");
        private static string mPathToSigner = Path.Combine( "tools", "SignApk.jar");
        private static string mPathToZipAlign = Path.Combine( "tools", "zipalign.exe");
        private static string mPathToKeyTool = Path.Combine("tools","KeyTool.jar");
        private static string mPathToXMLEditor = Path.Combine("tools", "axmleditor.jar");

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
        /// Check the valibility of user\apktool\framework\1.apk 
        /// </summary>
        public static void prepare()
        {
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
            cmd.Add(string.Format("\"{0}\"",store_pw));
            cmd.Add(string.Format("\"{0}\"",alias));
            cmd.Add(string.Format("\"{0}\"",key_pw));

            int i = 0;
            Sys.Run( cmd.ToCommand(), (I, E) =>
            {
                int.TryParse(E, out i);
            });

            return i;
        }

        /// <summary>
        /// axmleditor will output a file named 'axml_%s.xml'
        /// usage:axmleditor axml.xml -dir xml_temp test_chan
        /// </summary>
        /// <param name="srcaxml"></param>
        /// <param name="chan"></param>
        public static void EditorAXML(string srcaxml, string dir,string chan)
        {
            List<string> cmd = new List<string>();

            cmd.Add("java");
            cmd.Add("-jar");
            cmd.Add(mPathToXMLEditor);
            cmd.Add(srcaxml);
            cmd.Add("-dir");
            cmd.Add(dir);
            cmd.Add(chan);

            Sys.Run(cmd.ToCommand());
        }

        /// <summary>
        /// Extract AndroidManifest.xml file to destiny directory
        /// </summary>
        /// <param name="apk">apk file</param>
        /// <param name="dstdir">extract folder</param>
        public static void ExtractAXML(string apk, string dstdir)
        {
            using (ZipFile zip = ZipFile.Read(apk))
            {
                ZipEntry e = zip["AndroidManifest.xml"];
                e.Extract(dstdir, ExtractExistingFileAction.OverwriteSilently);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apk">apk file</param>
        /// <param name="axml"></param>
        public static void UpdateAXML(string apk, string axml)
        {
            using (ZipFile zip = ZipFile.Read(apk))
            {
                zip.UpdateFile(axml,"AndroidManifest.xml");
            }
        }

        public static void ZipApk(string dir, string apk)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.ParallelDeflateThreshold = -1;
                zip.AddDirectory(dir);
                zip.Save(apk);
            }
        }

        public static void UpzipApk(string apk, string dir)
        {
            System.Diagnostics.Debug.WriteLine(":" + System.Environment.CurrentDirectory);
            using (ZipFile zip = ZipFile.Read(apk))
            {
                foreach (ZipEntry e in zip)
                {
                    e.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                }
            }
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
            cmd.Add(string.Format("\"{0}\"", pathToApk ));

            string data = null;

            Sys.Run(cmd.ToCommand(), (I,E) =>
            {
                data = I;
               
            });
            return data;
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
            cmd.Add(string.Format("\"{0}\"",storepw));
            cmd.Add(string.Format("\"{0}\"",entry));
            cmd.Add(string.Format("\"{0}\"",keypw));

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
