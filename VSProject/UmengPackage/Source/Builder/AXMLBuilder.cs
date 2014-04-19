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
        }

        public override void BuildUnsignedApk(string channel)
        {
            Aapt.EditorAXML(COPY_AXML, WORK_SPACE, channel);
            string output = Path.Combine(WORK_SPACE,string.Format("axml_{0}.xml",channel));
            File.Copy(output, ORIGIN_AXML,true);

            Aapt.ZipApk(ORIGIN_APK, GetUnsignedApk());
        }

    }
}
