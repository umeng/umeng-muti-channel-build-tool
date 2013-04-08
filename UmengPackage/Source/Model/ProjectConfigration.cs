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
    [Serializable]
    public class ProjectConfigration: INotifyPropertyChanged
    {
        //setting setting@project.xml
        //private const string setting = "setting";

        //Directory of config file
        public static string Config_Path { get; set; }
        //Channels List
        public ObservableCollection<EditItem> Candinate = new ObservableCollection<EditItem>();


        private string android_path;
        public string Android_Path
        {
            get { return android_path; }
            set
            {
                if (value != android_path)
                {
                    android_path = value;
                    NotifyPropertyChanged("Android_Path");
                }
            }
        }
        private string java_path;
        public string Java_Path
        {
            get{ return java_path;}
            set{
                if( java_path!= value)
                {
                    java_path = value;
                    NotifyPropertyChanged("Java_Path");
                }
            }
        }
        //sign

        private string keystore_file_path ;
        public string Keystore_File_Path
        {
            get { return keystore_file_path; }
            set
            {
                if (value != keystore_file_path)
                {
                    keystore_file_path = value;
                    NotifyPropertyChanged("Keystore_File_Path");
                }
            }
        }
        private string keystore_pw ;
        public string Keystore_Pw
        {
            get { return keystore_pw; }
            set
            {
                if (value != keystore_pw)
                {
                    keystore_pw = value;
                    NotifyPropertyChanged("Keystore_Pw");
                }
            }
        }
        private string key_pw ;
        public string Key_Pw
        {
            get { return key_pw; }
            set
            {
                if (value != key_pw)
                {
                    key_pw = value;
                    NotifyPropertyChanged("Key_Pw");
                }
            }
        }

        private string alias ;
        public string Alias 
        {
            get { return alias; }
            set
            {
                if (value != alias)
                {
                    alias = value;
                    NotifyPropertyChanged("Alias");
                }
            }
        }

        // should proguard
        public bool setProguard { get; set; }
        // tools path

        static ProjectConfigration()
        {
            Config_Path = Path.Combine(System.Environment.CurrentDirectory, "projects");
        }
        public ProjectConfigration()
        {
        }

        

        public bool addChannel(EditItem new_channel)
        {
            if (Candinate.Contains(new_channel))
            {
                return false;
            }
            else
            {
                Candinate.Add(new_channel);
                return true;
            }
        }

        public bool removeChannle(EditItem channel)
        {
            return Candinate.Remove(channel);
        }

        public string getInput()
        {
            return Config_Path;
        }

        public string getOutput(string channle)
        {
            throw new NotImplementedException();
        }
        
       

        //project_path @ D://wwwww//demo/

        public void writeSettintToFile(string projectName)
        {
            if (!Directory.Exists(Config_Path))
            {
                Directory.CreateDirectory(Config_Path);
            }

            string project_path = System.IO.Path.Combine(Config_Path, string.Format("setting@{0}.xml", projectName));

            if (File.Exists(project_path)) File.Delete(project_path);

            using (Stream file = File.Open(project_path, FileMode.OpenOrCreate))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, this);
            }
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
            string project_path = System.IO.Path.Combine(Config_Path, string.Format("setting@{0}.xml",projectName));

            if (!File.Exists(project_path))
            {
                return null;
            }
            ProjectConfigration c;
            using (FileStream fs = new FileStream(project_path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                c = formatter.Deserialize(fs) as ProjectConfigration;
            }

            if (c == null)
            {
                c = new ProjectConfigration();
            }

            return c;
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {

            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(info));

            }

        }
    }
}