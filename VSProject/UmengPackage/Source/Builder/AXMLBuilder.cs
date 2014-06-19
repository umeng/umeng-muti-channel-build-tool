using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UmengPackage.Source.Common;
using UmengPackage.Source.Model;
using CommonTools;
using System.ComponentModel;
using System.IO;

namespace UmengPackage.Source
{
    /// <summary>
    /// dir/temp/decode-folder
    /// dir/xml_temp/apk/decode-folder
    /// dir/xml_temp/origin.xml
    /// dir/xml_temp/modified.xml
    /// dir/apk_temp/unsigned-{application}.apk
    /// dir/apk_temp/unziplaligned-{application}.apk
    /// dir/apk_temp/{application}-channel.apk
    /// dir/tools/apktool/.bat
    /// </summary>
    class AXMLBuilder : Builder
    {
        private const string WORK_SPACE = "xml_temp";
        private static readonly string ORIGIN_APK = Path.Combine(WORK_SPACE, "apk");
        private static readonly string ORIGIN_AXML = Path.Combine(ORIGIN_APK, "AndroidManifest.xml");
        private static readonly string COPY_AXML = Path.Combine(WORK_SPACE, "copy.xml");
        private static readonly string CANDINATE_AXML = Path.Combine(WORK_SPACE, "AndroidManifest.xml");
        private static readonly string CANDINATE_APK = Path.Combine(WORK_SPACE, "copy.apk");
        private static readonly string CANDINATE_APK1 = Path.Combine(WORK_SPACE, "copy1.apk");
        private static readonly string COPY_AAPT = Path.Combine(WORK_SPACE, "aapt.exe");
        private static string mPathToAapt = Path.Combine( "tools", "apktool", "aapt.exe");
      
        private string mPathToApk;

        public AXMLBuilder(ProjectConfigration config,string apk, BackgroundWorker monitor, DoWorkEventArgs e) : base(config, apk.ToFileName(), monitor,e)
        {
            mPathToApk = apk;
        }

        public override void Backup()
        {
        }
        public override void Restore()
        {
        }

        public override void SetProjectEnvironmet()
        {
            DirectoryInfo di = new DirectoryInfo(WORK_SPACE);
            if (di.Exists)
            {
                di.Delete(true);
            }
            Aapt.UpzipApk(mPathToApk, ORIGIN_APK );
            File.Copy(ORIGIN_AXML, COPY_AXML,true);
            File.Copy(mPathToApk, CANDINATE_APK, true);
            File.Copy(mPathToAapt, COPY_AAPT);
        }

        public override void BuildUnsignedApk(string channel)
        {
            Aapt.EditorAXML(COPY_AXML, WORK_SPACE, channel);
            string output = Path.Combine(WORK_SPACE,string.Format("axml_{0}.xml",channel));

            //rename *.xml to AndroidManifest.xml
            File.Delete(CANDINATE_AXML);
            File.Move(output, CANDINATE_AXML);
            //copy copy.apk to copy1.apk
            File.Copy(CANDINATE_APK, CANDINATE_APK1, true);
            //aapt r copy1.apk AndroidManifest.xml
            removeAXML(CANDINATE_APK1);
            //aapt a copy1.apk AndroidManifest.xml
            addAXML(CANDINATE_APK1);
            //copy copy1.apk to unsigned apk
            File.Copy(CANDINATE_APK1, GetUnsignedApk(), true);
        }

        private void removeAXML(string apk){
            if (!File.Exists(apk))
            {
                throw new Exception("Target apk is missing..");
            }

            List<String> cmd = new List<string>();
         
            cmd.Add(COPY_AAPT);
            cmd.Add("r");
            cmd.Add(apk);
            cmd.Add("AndroidManifest.xml");
         
            Sys.Run(cmd.ToCommand());
        }

        /// <summary>
        /// TODO: refactor
        /// </summary>
        /// <param name="apk"></param>
        private void addAXML(string apk)
        {
            if (!File.Exists(apk))
            {
                throw new Exception("Target apk is missing..");
            }

            var cd = System.Environment.CurrentDirectory;
            try
            {
                System.Environment.CurrentDirectory = WORK_SPACE;

                List<String> cmd = new List<string>();

                cmd.Add("aapt.exe");
                cmd.Add("a");
                cmd.Add("copy1.apk");
                cmd.Add("AndroidManifest.xml");
                Sys.setRedirect(false);
                Sys.Run(cmd.ToCommand());
            }
            finally
            {
                Sys.setRedirect(true);
                System.Environment.CurrentDirectory = cd;
            }
        }

    }
}
