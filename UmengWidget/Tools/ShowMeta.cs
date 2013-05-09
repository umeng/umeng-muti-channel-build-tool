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
    class Component
    {
        public string Feature;
        public bool Mask;

        public string Name;

        public Component(string name, string feature)
        {
            Name = name;
            Feature = feature;

            Mask = false;
        }

        public bool Match(string text)
        {
            if (!Mask && text.Contains(Feature))
            {
                Mask = true;
            }

            return Mask;
        }
    }
    /// <summary>
    /// Get:
    /// Appkey,
    /// Channel
    /// </summary>
    class ShowMeta
    {
        private UmengMeta Meta = new UmengMeta();

        int[] arr1Line = {1, 2, 3, 4, 5};
        
        private Component[] Componets = {   new Component("统计分析","http://alog.umeng.com/app_logs"),
                                            new Component("交换网络", "http://ex.puata.info"),
                                            new Component("双向反馈","http://feedback.whalecloud.com/feedback"),
                                            new Component("自动更新","http://au.umeng.com/api/check_app_update")
                                        };

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

            for (int i = 0; i < Componets.Length; i++)
            {
                if (Componets[i].Mask)
                {
                    b.Append(Componets[i].Name);
                    b.Append("    ");
                }
            }

            Meta.Components = b.ToString();

            System.Diagnostics.Debug.WriteLine(Meta.Components);
        }

        private void seek(string root)
        {
            if (Componets[0].Mask && Componets[1].Mask && Componets[2].Mask && Componets[3].Mask)
            {
                return;
            }

            string []files = Directory.GetFiles(root);

            foreach (string file in files)
            {
                string content = File.ReadAllText(file);

                foreach( Component c in Componets)
                {
                    c.Match(content);
                }
            }

            foreach (string folder in Directory.GetDirectories(root))
            {
                seek(folder);
            }
        }
    }
}
