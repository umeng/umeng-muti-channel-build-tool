using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using CommonTools;

namespace UmengWidget.Common
{
    public class Apktool
    {
        private static readonly string TEMP = "widget_temp";

        public static DecodedApkStruct Decode(string apk)
        {
            Aapt.DecodeApkWithSource(apk, TEMP);

            return new DecodedApkStruct( TEMP ).parseAxml();
        }

        public static void Build(string dstApk)
        {
            Aapt.BuildApk(TEMP, dstApk);
        }
    }
}
