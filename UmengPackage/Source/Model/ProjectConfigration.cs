using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;

using UmengPackage.Source.Common;

namespace UmengPackage.Source.Model
{
    /// <summary>
    /// Description of Configration. used to parse old setting files
    /// Save config at {dir}/projects/setting@{profile}.xml
    /// </summary>
    public class ProjectConfigration
    {
        //setting setting@project.xml
        //private const string setting = "setting";
        //Directory of config file
        public static string Config_Path { get; set; }
        
        //Channels List
        public List<string> Candinate = new List<string>();

        public string KeystoreFilePath;
        public string KeystorePassword;
        public string AliasPassword;
        public string Alias;

        static ProjectConfigration()
        {
            Config_Path = "projects";
        }

        public ProjectConfigration(){}
        
        public void WriteSettintToFile(string projectName)
        {
            if (!Directory.Exists(Config_Path))
            {
                Directory.CreateDirectory(Config_Path);
            }

            string filename = System.IO.Path.Combine(Config_Path, string.Format("setting@{0}.xml", projectName));

            if (File.Exists(filename)) File.Delete(filename);

            var editor = Preferences.getPreferences(filename).Editor();

            editor.WriteString("keystore", KeystoreFilePath)
                  .WriteString("keystore_password", KeystorePassword)
                  .WriteString("alias", Alias)
                  .WriteString("alias_password", AliasPassword)
                  .WriteList("channels", Candinate)
                  .Commit();
        }

        public static string[] GetConfigFileList()
        {
            if (!Directory.Exists(Config_Path))
            {
                return null;
            }
            string [] files = Directory.GetFiles(Config_Path);
            string [] configs = files.Where(T => Path.GetFileName(T).isConfigFile())
                                    .Select( T => Path.GetFileName(T).ToConfigFileName())
                                    .ToArray();
            return configs;
        }

        public static ProjectConfigration readSettingFromFile(string projectName)
        {

            string filename = System.IO.Path.Combine(Config_Path, string.Format("setting@{0}.xml",projectName));

            if (!File.Exists(filename))
            {
                return null;
            }

            var preference = Preferences.getPreferences(filename);

            if( preference == null)
            {
                return null;
            }

            var config = new ProjectConfigration();

            config.KeystoreFilePath = preference.getString("keystore");
            config.KeystorePassword = preference.getString("keystore_password");
            config.Alias = preference.getString("alias");
            config.AliasPassword = preference.getString("alias_password");

            config.Candinate = preference.getList("channels");

            return config;
        }

        public static List<Dictionary<String,String>> GetTemplate()
        {
            string filename = System.IO.Path.Combine(Config_Path, string.Format("template.xml"));

            if (!File.Exists(filename))
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            var channel = doc.GetElementsByTagName("channels")[0];
            var list = new List<Dictionary<string, string>>();

            foreach (XmlNode item in channel.ChildNodes)
            {    
                var dic = new Dictionary<string, string>();

                dic.Add( "id" , item.Attributes["id"].Value as string);
                dic.Add( "cat", item.Attributes["cat"].Value as string);
                dic.Add("name", item.InnerText);

                list.Add( dic);
            }

            return list;
        }
    }
}