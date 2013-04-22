using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace CommonTools
{
    public static class UmengExtention
    {
        public static string ToCommand(this List<string> cmd)
        {

            StringBuilder msb = new StringBuilder();

            foreach (string p in cmd)
            {
                msb.Append(p);
                msb.Append(" ");
            }

            return msb.ToString();
        }

        public static bool isApkFile(this string apkFile)
        {
            return apkFile.ToLower().EndsWith(".apk");
        }

        public static string formatFileSize(this string filename)
        {
            string[] sizes = { "B", "K", "M", "G" };
            double len = new FileInfo(filename).Length;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static T XamlClone<T>(this T original) where T : class
        {
            if (original == null)
                return null;

            object clone;
            using (var stream = new MemoryStream())
            {
                XamlWriter.Save(original, stream);
                stream.Seek(0, SeekOrigin.Begin);
                clone = XamlReader.Load(stream);
            }

            if (clone is T)
                return (T)clone;
            else
                return null;
        }
    }
}
