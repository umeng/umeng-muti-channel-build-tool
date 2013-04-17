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
        private UmengMeta Meta;

        private string[] feature = {
                                    "http://alog.umeng.com/app_logs",
                                    "http://au.umeng.com/api/check_app_update",
                                    "http://feedback.whalecloud.com/feedback",
                                    "http://ex.puata.info"
                                    };

        private bool[] mask = { false, false, false, false };

        public UmengMeta run(string apk)
        {
            var apkStruct = Apktool.Decode(apk);

            parseAppkeyAndChannel(apkStruct.AxmlFile);
            parsePackge(apkStruct.Smali);

            return Meta;
        }

        private void parseAppkeyAndChannel(string pathToXaml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathToXaml);

            //update 
            XmlNodeList mata_datas = doc.GetElementsByTagName("meta-data");

            foreach (XmlElement mata_data in mata_datas)
            {
                if (mata_data.GetAttribute("android:name").Equals("UMENG_CHANNEL"))
                {
                    Meta.Appkey = mata_data.GetAttribute("android:value");

                    continue;
                }

                if (mata_data.GetAttribute("android:name").Equals("UMENG_APPKEY"))
                {
                    Meta.Channel = mata_data.GetAttribute("android:value");

                    continue;
                }
            }
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

            if (mask[0])
            {
                Meta.Analytics = Visibility.Visible;
            }
            if (mask[1])
            {
                Meta.Update = Visibility.Visible;
            }

            if (mask[2])
            {
                Meta.Feedback = Visibility.Visible;
            }

            if (mask[3])
            {
                Meta.XP = Visibility.Visible;
            }
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
