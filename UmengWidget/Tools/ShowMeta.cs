using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UmengWidget.Common;
using System.Xml;
using CommonTools;
using UmengWidget.Model;
using System.Windows;

namespace UmengWidget.Tools
{
    /// <summary>
    /// Get:
    /// Appkey,
    /// Channel
    /// </summary>
    class ShowMeta
    {
        private UmengMeta Meta = new UmengMeta();

        private string[] feature = {
                                    "http://alog.umeng.com/app_logs",
                                    "http://au.umeng.com/api/check_app_update",
                                    "http://feedback.whalecloud.com/feedback",
                                    "http://ex.puata.info"
                                    };

        private bool[] mask = { false, false, false, false };
        private string[] componets = {"统计分析","交换网络","分享组件","双向反馈","自动更新" };

        public UmengMeta run(string apk)
        {
            var apkStruct = Apktool.Decode(apk);

            Meta.Appkey = apkStruct.Appkey;
            Meta.Channel = apkStruct.Channel;

            parsePackge(apkStruct.Smali);

            return Meta;
        }

       

        private void parsePackge(string root)
        {
            string umeng_root = Path.Combine("com", "umeng");
            if (Directory.Exists( umeng_root ))
            {
                seek(umeng_root);
            }
            else
            {
                seek(root);
            }

            StringBuilder b = new StringBuilder();

            for( int i = 0; i< mask.Length; i ++)
            {
                if (mask[i])
                {
                    b.Append(componets[i]);
                    b.Append("    ");
                }
            }

            Meta.Components = b.ToString();

            System.Diagnostics.Debug.WriteLine(Meta.Components);
        }

        private void seek(string root)
        {
            if (mask[0] && mask[1] && mask[2] && mask[3])
            {
                return;
            }

            string []files = Directory.GetFiles(root);

            foreach (string file in files)
            {
                string content = File.ReadAllText(file);

                for(int i = 0; i < feature.Length; i ++)
                {
                    if( content.Contains(feature[i]))
                    {
                        mask[i] = true;
                    }
                }
            }

            foreach (string folder in Directory.GetDirectories(root))
            {
                seek(folder);
            }
        }
    }
}
