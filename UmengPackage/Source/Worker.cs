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
using CommonTools;

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
        DecodedApkStruct project;

		public static Worker Instanse(){
            return worker;
		}
		public Worker(){}

        public Worker setConfigure(ProjectConfigration c)
        {
			config = c;
            return this;
		}

        public Worker setMoniter(System.ComponentModel.BackgroundWorker bw)
        {
            monitor = bw;
            return this;
        }

        public Worker setProject(DecodedApkStruct apkStruct)
        {
            project = apkStruct;
            return this;
        }
		
		public void start(){
			worker.run();
		}

		private void run(){
            new ApkBuilder(config, project , monitor).Build();
		}
	}
}
