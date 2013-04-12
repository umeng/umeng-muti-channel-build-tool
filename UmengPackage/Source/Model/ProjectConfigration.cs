using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Collections.ObjectModel;

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
            Config_Path = Path.Combine(System.Environment.CurrentDirectory, "projects");
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

        public static List<string> GetTemplate()
        {
            string filename = System.IO.Path.Combine(Config_Path, string.Format("template.xml"));

            if (!File.Exists(filename))
            {
                return null;
            }

            var preference = Preferences.getPreferences(filename);

            if (preference != null)
            {
                return preference.getList("channels");
            }
            else
            {
                return null;
            }
        }
    }
}