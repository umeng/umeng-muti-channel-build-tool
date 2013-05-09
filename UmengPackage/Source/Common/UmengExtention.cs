using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace UmengPackage.Source.Common
{
    static class UmengExtention
    {

        public static string ToSafePathString(this String path)
        {
            return string.Format("\"{0}\"", path);
        }

        /// <summary>
        /// Folder , dir/apkfolder/ return apkolder
        /// File, dir/bin/123.apk/ return 123
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToFileName(this String path)
        {
            if( Directory.Exists(path))
            {
                return Path.GetDirectoryName( path );
            }

            string fileNameWidthSuffix = System.IO.Path.GetFileName(path).ToLower();

            int length = fileNameWidthSuffix.Length - ".apk".Length;

            return fileNameWidthSuffix.Substring(0, length);
        }
        /// <summary>
        /// setting@123.xml return 123
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public static string ToConfigFileName(this String configFile)
        {
            Regex r = new Regex(@"setting@(.*).xml");

            return r.Match(configFile).Groups[1].Value;
        }

        public static bool isConfigFile(this string configFile)
        {
            return configFile.StartsWith("setting@");
        }

        public static ChannelItem find(this ObservableCollection<ChannelItem> list, string name)
        {
            ChannelItem target = null;

            foreach (ChannelItem item in list)
            {  
                if( item.isSameChannel( name))
                {
                    target = item;
                    break;
                } 
            }

            return target;
        }



        public static int findIndex(this ObservableCollection<ChannelItem> list, string name)
        {
            int index = -1 ;
            
            for(int i =0; i < list.Count; i++)
            {
                var item = list[i];

                if (item.isSameChannel( name))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public static bool deleteByName(this ObservableCollection<ChannelItem> list, string name)
        {
            bool isSuccess = false;

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];

                if (item.isSameChannel(name))
                {
                    isSuccess = true;
                    list.RemoveAt(i);
                    break;
                }
            }
            return isSuccess;
        }

        public static bool isSameChannel(this ChannelItem item,string cname)
        {
            var name = cname.Trim();
            if (item.ItemName.Equals(name))
            {
                return true;
            }

            if( (item is TemplateItem) && (item as TemplateItem).Id.Equals( name))
            {
                return true;
            }

            return false;
        }
     
    }
}
