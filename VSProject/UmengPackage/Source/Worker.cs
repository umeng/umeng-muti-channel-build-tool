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
        private static Builder builder;

        //BackgroudWorker monitor ,used to publish progress
        BackgroundWorker monitor;
        //Project Configration used to sign and zipAlign apk
		ProjectConfigration config;
        //report result
        DoWorkEventArgs result;

        public static Worker getInstance(ProjectConfigration c, System.ComponentModel.BackgroundWorker bw, DoWorkEventArgs e)
        {
            return new Worker(c, bw,e);
		}

        public Worker(ProjectConfigration c, System.ComponentModel.BackgroundWorker bw, DoWorkEventArgs e) 
        {
            config = c;
            monitor = bw;
            result = e;
        }

        public void setApkBuilder(DecodedApkStruct apkStruct)
        {
            builder = new ApkBuilder(config, apkStruct, monitor,result);
		}

        public void setXMLBuilder(string apk)
        {
            builder = new AXMLBuilder(config, apk, monitor, result);
        }

        public void start()
        {
            run();
        }

		private void run(){
            builder.Build();
		}
	}
}
