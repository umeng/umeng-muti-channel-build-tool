/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/7/17
 * Time: 17:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using System.ComponentModel;

using UmengPackage.Source.Common;
using UmengPackage.Source.Model;

namespace UmengPackage.Source
{
	/// <summary>
	/// All the build work is here
	/// </summary>
	public class Worker
	{
        private static Worker worker = new Worker();

        //BackgroudWorker monitor ,used to publish progress
        BackgroundWorker monitor;
        //Project Configration used to sign and zipAlign apk
		ProjectConfigration config;
        //Path to Apk file or to Source folder
        String project;

		public static Worker Instanse(){
            return worker;
		}
		public Worker(){}

        public Worker setProfile(ProjectConfigration c)
        {
			config = c;
            return this;
		}

        public Worker setMoniter(System.ComponentModel.BackgroundWorker bw)
        {
            monitor = bw;
            return this;
        }

        public Worker setProject(String p)
        {
            project = p;
            return this;
        }
		
		public void start(){
			worker.run();
		}

		private void run(){
            //Builder builder = new ApkBuilder( config, project, project.ToFileName(),monitor);
            //builder.Build();
            PackageState state = new PackageState();

            for (int i = 0; i < 10; i++)
            {
                state.setChannel( "Channel:"+i);
                monitor.ReportProgress( i , state.setState( State.START));

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                monitor.ReportProgress( i , state.setState( State.END));

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

            }
		}
	}
}
