using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UmengWidget.Common;
using CommonTools;

namespace UmengWidget.Tools
{
    class GenDebugApk
    {
        public static void run(string apk)
        {
            var dfs = Apktool.Decode(apk);

            string[] smali = File.ReadAllLines(dfs.LogFile);

            for(int i = 0; i< smali.Length; i++){
                string line = smali[i];

                if (line.Trim().Equals("const/4 v0, 0x0"))
                {
                    smali[i] = line.Replace("0x0","0x1");
                    break;
                }
            }

            File.Delete(dfs.LogFile);
            File.WriteAllLines( dfs.LogFile, smali);

            //Debug_ap
            Apktool.Build("DEBUG_" + apk);
        }
    }
}
