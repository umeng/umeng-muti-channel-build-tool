using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

using UmengPackage.Source.Common;
using UmengPackage.Source.Model;
using CommonTools;

namespace UmengPackage.Source
{
    /// <summary>
    /// Deprecated
    /// </summary>
    class SourceBuilder:Builder
    {

        private string[] srcs = { "AndroidManifest.xml", "ant.properties", "build.xml", "project.properties" };
        private ProjectStruct projectStruct;

        public SourceBuilder(ProjectConfigration config, String projectName, String sourceFolder, BackgroundWorker workReporter)
            : base(config, projectName, workReporter)
        {
            projectStruct = new ProjectStruct(sourceFolder);
        }

        /// <summary>
        /// AndroidManifest.xml , ant.properties, build.xml,project.properties should backup
        /// </summary>
        public override void Backup()
        {
            string src_file = null;
            string dst_file = null;
            foreach (string src in srcs.Where(X => File.Exists(Path.Combine(projectStruct.Root, X))))
            {

                src_file = Path.Combine(projectStruct.Root, src);
                dst_file = src_file + "~";

                if (File.Exists(dst_file)) File.Delete(dst_file);

                File.Copy(src_file, src_file + "~");

                if (src.Equals(srcs[1]))
                {
                    File.Delete(src_file);
                }
            }

        }

        /// <summary>
        /// if original file exits , so has backup file , or delete the original file
        /// </summary>
        public override void Restore()
        {
            string dst_file = null;
            string src_file = null;
            foreach (string src in srcs)
            {
                dst_file = Path.Combine(projectStruct.Root, src + "~");
                src_file = Path.Combine(projectStruct.Root, src);

                //restore backup file
                if (File.Exists(dst_file) && File.Exists(src_file))
                {
                    File.Replace(dst_file, src_file, null);

                }//restore deleted file
                else if (File.Exists(dst_file) && !File.Exists(src_file))
                {
                    File.Move(dst_file, src_file);
                }//delete App generated file
                else if (!File.Exists(dst_file) && File.Exists(src_file))
                {
                    File.Delete(src_file);
                }

            }
        }

        public override void BuildUnsignedApk()
        {
            Sys.Run(string.Format("ant clean -f {0}", projectStruct.BuildFile ));
            Sys.Run(string.Format("ant release -f {0}", projectStruct.BuildFile ));
        }

        public override void SetProjectEnvironmet()
        {
            //Log.i("set environment");

            //string pathOrg = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);

            //System.Environment.SetEnvironmentVariable("JAVA_HOME", java_home);//, EnvironmentVariableTarget.User);
            //System.Environment.SetEnvironmentVariable("ANT_HOME", ant_home);//, EnvironmentVariableTarget.User);


            //List<String> path = new List<string>();

            //path.Add(pathOrg);
            //path.Add(Path.Combine("%JAVA_HOME%", "bin"));
            //path.Add(Path.Combine("%JAVA_HOME%", "lib"));
            //path.Add(Path.Combine("%ANT_HOME%", "bin"));
            //path.Add("%JAVA_HOME%");
            //path.Add("%ANT_HOME%");
            //path.Add(Path.Combine(java_home, "bin"));
            //path.Add(Path.Combine(ant_home, "bin"));
            //path.Add(Path.Combine(android_home, "tools"));
            //path.Add(apktool_home);

            //System.Text.StringBuilder paths = new System.Text.StringBuilder();

            //foreach (string p in path)
            //{
            //    paths.Append(p);
            //    paths.Append(";");
            //}

            //System.Environment.SetEnvironmentVariable("PATH", paths.ToString());       

            throw new NotImplementedException();
        }

        public override string GetAndroidManifestPath()
        {
            return projectStruct.AXMLFile;
        }

        public override string GetUnsignedApk()
        {

            string bin = Path.Combine(CurrentDir, "bin");
			string [] files = Directory.GetFiles( bin );

			foreach(string file_name in files){
				if(file_name.EndsWith("unsigned.apk")){
					return Path.Combine(bin,file_name);
				}
			}

			throw new Exception("build fail , can't find *unsigned.apk file.");
        }

        public override string GetUnzipAlignedApk()
        {
            return Path.Combine( CurrentDir,"bin", String.Format("unzipalign-{0}.apk",ApplicationName));
        }

        public override string GetFinalApk(string channel)
        {
            return Path.Combine( CurrentDir,"bin", String.Format("{0}-{1}.apk", ApplicationName, channel));
        }

        private void setProguard()
        {
            string projectproperties_file = projectStruct.PPFile;

            if (!File.Exists(projectproperties_file))
            {
                Log.i("Can't find project.properties file");
                return;
            }
            bool hasSetProguard = false;
            string line = null;
            using (StreamReader sr = File.OpenText(projectproperties_file))
            {
                while ((line = sr.ReadLine()) != null)
                {

                    if (line.StartsWith("#"))
                    {
                        continue;
                    }
                    else if (line.Contains("proguard.config"))
                    {//proguard.config=proguard.cfg
                        string proguard_config_file = line.Substring(line.IndexOf("=") + 1);

                        hasSetProguard = true;
                        break;
                    }//if
                }//while
            }//using

            if (!hasSetProguard && File.Exists( projectStruct.ProguardCfgFile ))
            {
                using (StreamWriter sw = File.AppendText(projectproperties_file))
                {
                    sw.WriteLine("proguard.config=proguard.cfg");
                }
            }
        }

        private void cancleProguard()
        {
            string projectproperties_file = projectStruct.PPFile;

            if (!File.Exists(projectproperties_file))
            {
                Log.i("Can't find project.properties file");
                return;
            }

            StringBuilder sb = new StringBuilder();
            string line = null;
            using (StreamReader sr = File.OpenText(projectproperties_file))
            {
                while ((line = sr.ReadLine()) != null)
                {

                    if (line.Contains("proguard.config"))
                    {//proguard.config=proguard.cfg
                        continue;
                    }
                    else
                    {
                        sb.Append(line);
                        sb.Append("\n");
                    }
                }//while
            }//using

            File.Delete(projectproperties_file);
            File.AppendAllText(projectproperties_file, sb.ToString());
        }


}
    /// <summary>
    /// dir
    /// --bin
    /// ----xxxunsigned.apk
    /// ----unzipAligned-{ApplicationName}.apk
    /// ----{ApplicationName}-{Channel}.apk
    /// --src
    /// --build.xml
    /// --AndroidManifest.xml
    /// --project.properties
    /// --proguard.cfg
    /// </summary>
    class ProjectStruct
    {
       public string Root { get; set; }
        
        public string BinFolder { get; set; }
        public string SrcFolder { get; set; }

        public string BuildFile { get; set; }
        public string AXMLFile { get; set; }
        public string PPFile { get; set; }
        public string ProguardCfgFile { get; set; }

        public ProjectStruct(string folder)
        {
            Root = folder;
        }

        private void initProject(string folder)
        {
            Root = folder;

            BinFolder = Path.Combine(Root, "bin");
            SrcFolder = Path.Combine(Root, "src");

            BuildFile = Path.Combine(Root, "Build.xml");
            AXMLFile = Path.Combine(Root, "AndroidManifest.xml");
            PPFile = Path.Combine(Root, "project.properties");
            ProguardCfgFile = Path.Combine( Root, "proguard.cfg");
                
        }

    }
}
