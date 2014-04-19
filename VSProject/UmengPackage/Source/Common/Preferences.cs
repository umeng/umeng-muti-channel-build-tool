using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UmengPackage.Source.Model
{
    public class Preferences
    {

        //public string KeyStore = null;
        //public string KeyStorePassword = null;
        //public string Alias = null;
        //public string AliasPassword = null;

        //public List<String> channels = new List<string>();
        private string fileName;
        public static Preferences getPreferences(string filename)
        {
            Preferences p = null;
            
            try
            {
                p = new Preferences(filename);
                p.parse();
            } 
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("parse error : " + e.Message);
            }

            return p;
        }

        public static Preferences getDefault()
        {
            return getPreferences("default.xml");
        }
        
        XmlDocument doc = new XmlDocument();
        Dictionary<string, object> dic = new Dictionary<string, object>();

        private Preferences(string filename)
        {
            fileName = filename;
        }

        private void parse()
        {
            doc.Load(fileName);

            foreach (XmlNode node in doc.LastChild.ChildNodes)
            {
                if (node.HasChildNodes && node.FirstChild.HasChildNodes)
                {
                    List<string> list = new List<string>();
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        list.Add(item.InnerText);
                    }

                    if (list.Count > 0)
                    {
                        dic.Add(node.Name, list);
                    }
                }
                else
                {
                    dic.Add(node.Name, node.InnerText);
                }
            }
        }

        public Editor Editor()
        {
            return new Editor(fileName);
        }

        public string getString(string key )
        {
            string value = null;
            if (dic.ContainsKey(key))
            {
                value = dic[key] as string;
            }

            return value;
        }

        public int? getInt(string key)
        {
            int? value = null;

            if (dic.ContainsKey(key))
            {
                value = int.Parse(dic[key] as string);
            }
            return value;
        }

        public List<String> getList(string key)
        {
            List<string> value = null;

            if (dic.ContainsKey(key))
            {
                value = dic[key] as List<string>;
            }

            return value;
        }
    }

    public class Editor
    {
        XmlTextWriter writer = null;
        
        const string Root = "root";

        public Editor(string filename)
        {
            writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement(Root);
        }

        public Editor WriteString(string key, string value)
        {
            writer.WriteElementString(key, value);
            return this;
        }

        public Editor WriteInt(string key, int value)
        {
            writer.WriteElementString(key, value.ToString());
            return this;
        }

        public Editor WriteList(string key, List<string> value)
        {
            writer.WriteStartElement(key);
            foreach (string item in value)
            {
                writer.WriteElementString("item", item);
            }
            writer.WriteEndElement();
            return this;
        }

        public void Commit()
        {
            writer.WriteEndElement();
            writer.Close();
        }
    }

}
