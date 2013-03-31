using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UmengWidget.Common;
using System.Xml;

namespace UmengWidget.Tools
{
    /// <summary>
    /// Get:
    /// Appkey,
    /// Channel
    /// </summary>
    class ShowMeta
    {
        public void run(string apk)
        {
            var dfs = Apktool.Decode(apk);

            XmlDocument doc = new XmlDocument();
            doc.Load(dfs.AxmlFile);

            //update 
            XmlNodeList mata_datas = doc.GetElementsByTagName("meta-data");
       
            string appkey = null;
            string channel = null;

            foreach (XmlElement mata_data in mata_datas)
            {
                if (mata_data.GetAttribute("android:name").Equals("UMENG_CHANNEL"))
                {
                    appkey = mata_data.GetAttribute("android:value");

                    continue;
                }

                if (mata_data.GetAttribute("android:name").Equals("UMENG_APPKEY"))
                {
                    channel = mata_data.GetAttribute("android:value");

                    continue;
                }
            }
            
        }
    }
}
